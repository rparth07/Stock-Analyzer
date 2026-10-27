using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Repository;
namespace Stock_Analyzer.Controllers;
[ApiController, Route("market-data")]
public class MarketDataController(StockAnalyzerContext db) : ControllerBase {
  [HttpGet("status")]
  public async Task<IActionResult> Status(CancellationToken token) => Ok(new {
    kind = "historical-end-of-day", currency = "INR", adjusted = false,
    sources = await db.ImportBatches.GroupBy(x => x.Source).Select(g => new { source = g.Key, tradingDate = g.Max(x => x.TradingDate), importedAt = g.Max(x => x.ImportedAt) }).ToListAsync(token)
  });
}
