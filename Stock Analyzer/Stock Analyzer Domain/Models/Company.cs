using Stock_Analyzer_Domain.Models.Filter;

namespace Stock_Analyzer_Domain.Models
{
  public class Company
  {
    public Guid Id { get; set; }

    public string Symbol { get; set; }

    public ICollection<BhavCopyInfo> BhavCopyInfos { get; set; } = new List<BhavCopyInfo>();

    public ICollection<BulkDeal> BulkDeals { get; set; } = new List<BulkDeal>();

    public List<FilterResult> FilterResults { get; set; } = new List<FilterResult>();

    public Company()
    { }

    public Company(string name)
    {
      this.Symbol = name;
    }
  }
}
