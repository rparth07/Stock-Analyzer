using FluentDateTime;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Service
{
  public class FilterService : IFilterService
  {
    private readonly IFilterRepository _filterRepository;
    private readonly IBulkDealRepository _bulkDealRepository;
    private readonly IBhavInfoRepository _bhavInfoRepository;
    public FilterService(IFilterRepository filterRepository,
                         IBhavInfoRepository bhavInfoRepository,
                         IBulkDealRepository bulkDealRepository)
    {
      _filterRepository = filterRepository;
      _bulkDealRepository = bulkDealRepository;
      _bhavInfoRepository = bhavInfoRepository;
    }

    public void AddFilter(Filter filter)
    {
      _filterRepository.AddFilter(filter);
    }

    public List<Filter> GetFilters()
    {
      return _filterRepository.GetFilters();
    }

    public Filter GetFilterByName(string filterName)
    {
      return _filterRepository.GetFilterByName(filterName);
    }

    public List<FilterResult> ExecuteFilter(Filter filter, DateTime filterDate)
    {
      var filterResults = _filterRepository.GetFilterResults(filter, filterDate);

      var bhavInfosOnCalculationDate = _bhavInfoRepository.GetAllBhavInfos(filterDate);
      var bulkDealOnCalculationDate = _bulkDealRepository.GetAllBulkDeals(filterDate);

      filterResults.ForEach(fr =>
      {
        fr.Company.BhavCopyInfos = bhavInfosOnCalculationDate
          .Where(_ => _.Company.Id.Equals(fr.Company.Id)).ToList();

        fr.Company.BulkDeals = bulkDealOnCalculationDate
          .Where(_ => _.Company.Id.Equals(fr.Company.Id)).ToList();
      });

      return filterResults;
    }
  }
}
