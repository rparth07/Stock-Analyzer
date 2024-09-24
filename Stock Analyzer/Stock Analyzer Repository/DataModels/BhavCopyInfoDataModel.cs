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

    [Required]
    public double PreviousClose { get; set; }

    [Required]
    public double OpenPrice { get; set; }

    [Required]
    public double HighPrice { get; set; }

    [Required]
    public double LowPrice { get; set; }

    [Required]
    public double LastPrice { get; set; }

    [Required]
    public double ClosePrice { get; set; }

    [Required]
    public double AvgPrice { get; set; }

    [Required]
    public double TtlTrdQnty { get; set; }

    [Required]
    public double TurnOverLacs { get; set; }

    [Required]
    public double NoOfTrades { get; set; }

    public double DeliveryQty { get; set; }

    public double DeliveryPercentage { get; set; }
  }
}