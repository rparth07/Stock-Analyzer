namespace Stock_Analyzer_Domain.Models
{
  public class BulkDeal
  {
    public Guid Id { get; set; }
    public Client Client { get; set; }
    public DateTime DealDate { get; set; }
    public Company Company { get; set; }
    public string CompanyFullName { get; set; }
    public StockAction StockAction { get; set; }
    public long Quantity { get; set; }
    public double TradePrice { get; set; }
    public string? Remarks { get; set; }
  }
}
