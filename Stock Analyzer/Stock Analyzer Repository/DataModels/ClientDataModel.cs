using System.ComponentModel.DataAnnotations;

namespace Stock_Analyzer_Repository.DataModels
{
  public class ClientDataModel
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public List<BulkDealDataModel> Deals { get; set; } = new List<BulkDealDataModel>();
  }
}