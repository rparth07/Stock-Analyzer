using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public List<FilterResult> GetFilterResultsToInsert(List<FilterResult> filterResults);
  }
}
