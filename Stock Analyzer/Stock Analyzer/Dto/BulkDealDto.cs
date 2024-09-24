using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.Dto
{
  public class BulkDealDto
  {
    public string ClientName { get; set; }

    public DateTime DealDate { get; set; }

    public string CompanySymbol { get; set; }

    public string CompanyFullName { get; set; }

    public StockAction StockAction { get; set; }

    public long Quantity { get; set; }

    public double TradePrice { get; set; }

    public string? Remarks { get; set; }
  }
}
