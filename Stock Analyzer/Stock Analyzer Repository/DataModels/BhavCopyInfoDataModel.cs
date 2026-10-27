using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Analyzer_Repository.DataModels
{
  public class BhavCopyInfoDataModel
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Series { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [ForeignKey("CompanyId")]
    [InverseProperty("BhavCopyInfos")]
    public virtual CompanyDataModel Company { get; set; }

    public double? PreviousClose { get; set; }

    public double? OpenPrice { get; set; }

    public double? HighPrice { get; set; }

    public double? LowPrice { get; set; }

    public double? LastPrice { get; set; }

    public double? ClosePrice { get; set; }

    public double? AvgPrice { get; set; }

    public double? TtlTrdQnty { get; set; }

    public double? TurnOverLacs { get; set; }

    public double? NoOfTrades { get; set; }

    public double? DeliveryQty { get; set; }

    public double? DeliveryPercentage { get; set; }
  }
}