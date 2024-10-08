using Stock_Analyzer_Domain.Models.Filter;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface IFilterRepository
  {
    public void AddFilter(Filter filter);
    public List<Filter> GetFilters();
    public Filter GetFilterByName(string filterName);
    public List<FilterCriteria> GetFilterCriterias(Filter filter);
    public List<FilterResult> GetFilterResults(Filter filter, DateTime filterDate);
    public void AddFilterResults(List<FilterResult> filterResults);
    public bool ExistFilterResultsFor(Filter filter, DateTime calculationDate);
    public void DeleteFilterByName(string filterName);
  }
}
