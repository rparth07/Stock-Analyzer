
namespace Stock_Analyzer_Domain.Models.Filter
{
  public class Filter
  {
    public Filter(string filterName)
    {
      FilterName = filterName;
      Criterias = new List<FilterCriteria>();
    }

    public Guid Id { get; set; }

    public string FilterName { get; set; }

    public List<FilterCriteria> Criterias { get; set; } = new List<FilterCriteria>();
  }
}
