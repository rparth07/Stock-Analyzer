using CsvHelper.Configuration.Attributes;

namespace Stock_Analyzer.CSVParserModel
{
  public class ParsedBulkDeal
  {
    [Index(0)]
    public DateTime DealDate { get; set; }
    [Index(1)]
    public string? CompanySymbol { get; set; }
    [Index(2)]
    public string? CompanyFullName { get; set; }
    [Index(3)]
    public string? ClientName { get; set; }
    [Index(4)]
    public string? StockAction { get; set; }
    [Index(5)]
    public string? Quantity { get; set; }
    [Index(6)]
    public string? TradePrice { get; set; }
    [Index(7)]
    public string? Remarks { get; set; }
  }
}
