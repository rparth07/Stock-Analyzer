using Stock_Analyzer_Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Analyzer_Repository.DataModels
{
  public class BulkDealDataModel
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("ClientId")]
    [InverseProperty("Deals")]
    public virtual ClientDataModel Client { get; set; }

    [Required]
    public DateTime DealDate { get; set; }

    [Required]
    [ForeignKey("CompanyId")]
    [InverseProperty("BulkDeals")]
    public virtual CompanyDataModel Company { get; set; }

    [Required]
    public string CompanyFullName { get; set; }

    [Required]
    public StockAction StockAction { get; set; }

    [Required]
    public long Quantity { get; set; }

    [Required]
    public double TradePrice { get; set; }

    public string? Remarks { get; set; }
  }
}