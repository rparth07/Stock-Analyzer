using Stock_Analyzer_Domain.Models.Filter;

namespace Stock_Analyzer_Service.Interface
{
  public interface IFilterService
  {
    public void AddFilter(Filter filter);

    public List<Filter> GetFilters();
    public Filter GetFilterByName(string filterName);

    public List<FilterResult> GetFilterResults(Filter filter, DateTime filterDate);
    public void DeleteFilterByName(string filterName);

    public void StoreFilterResultsByFilterFor(DateTime calculationDate);
  }
}
