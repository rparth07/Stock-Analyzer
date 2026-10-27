using System.Globalization;
using System.Net;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Repository;
using Stock_Analyzer_Repository.DataModels;
using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.MarketData;

public class NseArchiveImporter(HttpClient client, StockAnalyzerContext db) {
  public const string BhavSource = "NSE full bhavcopy archive";
  public const string BulkSource = "NSE bulk deals CSV";
  private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
  public static double? OptionalNumber(string? value) => double.TryParse(value?.Trim(), NumberStyles.Number, Culture, out var n) && double.IsFinite(n) ? n : null;
  private static double Number(string value) => OptionalNumber(value) ?? throw new InvalidDataException("NSE report contains an unavailable required numeric field.");
  public static DateTime TradingDate(string value) => DateTime.ParseExact(value.Trim(), new[] { "dd-MMM-yyyy", "dd-MM-yyyy", "yyyy-MM-dd" }, Culture, DateTimeStyles.None).Date;
  private static CsvReader Reader(string text) => new(new StringReader(text), new CsvConfiguration(Culture) { TrimOptions = TrimOptions.Trim });
  public async Task<int> ImportBhavAsync(DateTime date, CancellationToken token) {
    date = date.Date;
    if (await db.ImportBatches.AnyAsync(x => x.Source == BhavSource && x.TradingDate == date, token)) return 0;
    var url = $"https://nsearchives.nseindia.com/products/content/sec_bhavdata_full_{date:ddMMyyyy}.csv";
    using var response = await client.GetAsync(url, token);
    if (response.StatusCode == HttpStatusCode.NotFound) return 0; // Weekend, holiday or not published yet.
    response.EnsureSuccessStatusCode();
    var text = await response.Content.ReadAsStringAsync(token);
    using var csv = Reader(text); csv.Read(); csv.ReadHeader();
    if (csv.HeaderRecord?.FirstOrDefault() != "SYMBOL") throw new InvalidDataException("Unexpected NSE bhavcopy response.");
    var companies = await db.Company.ToDictionaryAsync(x => x.Symbol, token);
    var rows = new List<BhavCopyInfoDataModel>();
    while (csv.Read()) {
      string Field(string name) => csv.GetField(name)?.Trim() ?? "";
      if (TradingDate(Field("DATE1")) != date) throw new InvalidDataException("NSE report trading date does not match request.");
      var symbol = Field("SYMBOL"); var series = Field("SERIES");
      if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(series)) throw new InvalidDataException("NSE report is missing an identifier.");
      if (!companies.TryGetValue(symbol, out var company)) { company = new CompanyDataModel { Id = Guid.NewGuid(), Symbol = symbol }; companies.Add(symbol, company); db.Company.Add(company); }
      rows.Add(new BhavCopyInfoDataModel {
        Id = Guid.NewGuid(), Company = company, Series = series, Date = date,
        PreviousClose = OptionalNumber(Field("PREV_CLOSE")), OpenPrice = OptionalNumber(Field("OPEN_PRICE")), HighPrice = OptionalNumber(Field("HIGH_PRICE")),
        LowPrice = OptionalNumber(Field("LOW_PRICE")), LastPrice = OptionalNumber(Field("LAST_PRICE")), ClosePrice = OptionalNumber(Field("CLOSE_PRICE")),
        AvgPrice = OptionalNumber(Field("AVG_PRICE")), TtlTrdQnty = OptionalNumber(Field("TTL_TRD_QNTY")), TurnOverLacs = OptionalNumber(Field("TURNOVER_LACS")),
        NoOfTrades = OptionalNumber(Field("NO_OF_TRADES")), DeliveryQty = OptionalNumber(Field("DELIV_QTY")), DeliveryPercentage = OptionalNumber(Field("DELIV_PER"))
      });
    }
    if (rows.Count == 0) throw new InvalidDataException("NSE report had no records.");
    await using var transaction = await db.Database.BeginTransactionAsync(token);
    // Support an existing import without duplicating its prices.
    var existing = await db.BhavCopyInfo.Where(x => x.Date == date).Select(x => new { x.Company.Symbol, x.Series }).ToListAsync(token);
    var keys = existing.Select(x => (x.Symbol, x.Series)).ToHashSet();
    db.BhavCopyInfo.AddRange(rows.Where(x => !keys.Contains((x.Company.Symbol, x.Series))));
    db.ImportBatches.Add(new ImportBatch { Id = Guid.NewGuid(), Source = BhavSource, TradingDate = date, ImportedAt = DateTime.UtcNow, RowCount = rows.Count });
    await db.SaveChangesAsync(token); await transaction.CommitAsync(token); return rows.Count;
  }

  public async Task<int> ImportBulkAsync(CancellationToken token) {
    using var response = await client.GetAsync("https://nsearchives.nseindia.com/content/equities/bulk.csv", token);
    response.EnsureSuccessStatusCode();
    using var csv = Reader(await response.Content.ReadAsStringAsync(token)); csv.Read(); csv.ReadHeader();
    if (csv.HeaderRecord?.FirstOrDefault() != "Date") throw new InvalidDataException("Unexpected NSE bulk-deals response.");
    var parsed = new List<string[]>();
    while (csv.Read()) parsed.Add(Enumerable.Range(0, 8).Select(i => csv.GetField(i)?.Trim() ?? "").ToArray());
    if (parsed.Count == 0) return 0;
    var date = TradingDate(parsed[0][0]);
    if (parsed.Any(x => TradingDate(x[0]) != date)) throw new InvalidDataException("Mixed NSE bulk report dates.");
    if (await db.ImportBatches.AnyAsync(x => x.Source == BulkSource && x.TradingDate == date, token)) return 0;
    var companies = await db.Company.ToDictionaryAsync(x => x.Symbol, token);
    var clients = await db.Client.ToDictionaryAsync(x => x.Name, token);
    await using var transaction = await db.Database.BeginTransactionAsync(token);
    foreach (var fields in parsed) {
      var symbol = fields[1]; var name = fields[3];
      if (symbol.Length == 0 || name.Length == 0) throw new InvalidDataException("NSE bulk report missing identifier.");
      if (!companies.TryGetValue(symbol, out var company)) { company = new CompanyDataModel { Id = Guid.NewGuid(), Symbol = symbol }; companies.Add(symbol, company); db.Company.Add(company); }
      if (!clients.TryGetValue(name, out var person)) { person = new ClientDataModel { Id = Guid.NewGuid(), Name = name }; clients.Add(name, person); db.Client.Add(person); }
      var action = fields[4].ToUpperInvariant() switch { "BUY" => StockAction.Buy, "SELL" => StockAction.Sell, _ => throw new InvalidDataException("Unknown NSE deal side.") };
      db.BulkDeal.Add(new BulkDealDataModel { Id = Guid.NewGuid(), Company = company, Client = person, DealDate = date, CompanyFullName = fields[2], StockAction = action,
        Quantity = long.Parse(fields[5].Replace(",", ""), Culture), TradePrice = Number(fields[6]), Remarks = fields[7] == "-" ? null : fields[7] });
    }
    db.ImportBatches.Add(new ImportBatch { Id = Guid.NewGuid(), Source = BulkSource, TradingDate = date, ImportedAt = DateTime.UtcNow, RowCount = parsed.Count });
    await db.SaveChangesAsync(token); await transaction.CommitAsync(token); return parsed.Count;
  }
}
