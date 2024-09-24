using Stock_Analyzer_Repository.DataModels.Filter;
using System.ComponentModel.DataAnnotations;

namespace Stock_Analyzer_Repository.DataModels
{
  public class CompanyDataModel
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Symbol { get; set; }

    public ICollection<BhavCopyInfoDataModel> BhavCopyInfos { get; set; } = new List<BhavCopyInfoDataModel>();

    public ICollection<BulkDealDataModel> BulkDeals { get; set; } = new List<BulkDealDataModel>();

    public ICollection<FilterResultDataModel> FilterResults { get; set; } = new List<FilterResultDataModel>();

  }
}
