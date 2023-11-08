using FluentDateTime;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Service.CalculationType;
using Stock_Analyzer_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stock_Analyzer_Service
{
  public class FilterService : IFilterService
  {
    private readonly IFilterRepository _filterRepository;
    private readonly IBulkDealRepository _bulkDealRepository;
    private readonly IBhavInfoRepository _bhavInfoRepository;
    private readonly ICompanyRepository _companyRepository;
    public FilterService(IFilterRepository filterRepository,
                         IBhavInfoRepository bhavInfoRepository,
                         IBulkDealRepository bulkDealRepository,
                         ICompanyRepository companyRepository)
    {
      _filterRepository = filterRepository;
      _bulkDealRepository = bulkDealRepository;
      _bhavInfoRepository = bhavInfoRepository;
      _companyRepository = companyRepository;
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

    public List<FilterResult> GetFilterResults(Filter filter, DateTime filterDate)
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

    public void StoreFilterResultsByFilterFor(DateTime calculationDate)
    {
      var filters = _filterRepository.GetFilters();

      var companies = _companyRepository.GetAllCompanies();

      List<FilterResult> filterResultsToInsert = new List<FilterResult>();

      filters.ForEach(filter =>
      {
        var filterResults = ExecuteFilter(filter, calculationDate, companies);
        filterResultsToInsert.AddRange(filterResults);
      });

      _filterRepository.AddFilterResults(filterResultsToInsert);
    }

    private List<FilterResult> ExecuteFilter(Filter filter, DateTime calculationDate, List<Company> companies)
    {
      if(filter.FilterType == FilterType.MovingAverage)
      {
        return new MovingAverage(_bhavInfoRepository).ExecuteMovingAverageFilter(filter, calculationDate, companies);
      }

      return new List<FilterResult>();
    }
  }
}
