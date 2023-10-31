
namespace Stock_Analyzer_Domain.Models.Filter
{
  public class Filter
  {
    public Filter(string filterName, string series)
    {
      FilterName = filterName;
      Series = series;
      Criterias = new List<FilterCriteria>();
    }

    public Guid Id { get; set; }

    public string FilterName { get; set; }

    public string Series { get; set; }

    public List<FilterCriteria> Criterias { get; set; } = new List<FilterCriteria>();
  }
}
