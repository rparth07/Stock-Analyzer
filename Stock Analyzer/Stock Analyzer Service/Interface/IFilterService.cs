using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Service.Interface
{
  public interface IFilterService
  {
    public void AddFilter(Filter filter);

    public List<Filter> GetFilters();
    public Filter GetFilterByName(string filterName);

    public List<FilterResult> ExecuteFilter(Filter filter, DateTime filterDate);
  }
}
