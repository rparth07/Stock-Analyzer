using Microsoft.AspNetCore.Mvc;
using Stock_Analyzer.MarketData;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Repository;
namespace Stock_Analyzer.Controllers;
[ApiController, Route("market-data")]
public class MarketDataController(StockAnalyzerContext db, NseArchiveImporter importer) : ControllerBase {
  [HttpPost("import-bhav")]
  public async Task<IActionResult> ImportBhav(DateTime date, CancellationToken token) {
    var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Asia/Kolkata").Date;
    if (date.Date > today || date.Date < today.AddYears(-1)) return BadRequest(new { message = "Choose a published trading date within the last year." });
    var count = await importer.ImportBhavAsync(date.Date, token);
    if (count == 0 && !await db.ImportBatches.AnyAsync(x => x.Source == NseArchiveImporter.BhavSource && x.TradingDate == date.Date, token))
      return NotFound(new { message = "NSE has not published a report for this date. It may be a holiday or before publication." });
    return Ok(new { imported = count });
  }
  [HttpPost("import-bulk")]
  public async Task<IActionResult> ImportBulk(DateTime date, CancellationToken token) {
    var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Asia/Kolkata").Date;
    if (date.Date != today) return UnprocessableEntity(new { message = "Only the latest NSE bulk-deal snapshot can be imported. Previously imported dates remain searchable." });
    var count = await importer.ImportBulkAsync(token);
    return Ok(new { imported = count });
  }
  [HttpGet("status")]
  public async Task<IActionResult> Status(CancellationToken token) => Ok(new {
    kind = "historical-end-of-day", currency = "INR", adjusted = false,
    sources = await db.ImportBatches.GroupBy(x => x.Source).Select(g => new { source = g.Key, tradingDate = g.Max(x => x.TradingDate), importedAt = g.Max(x => x.ImportedAt) }).ToListAsync(token)
  });
}
