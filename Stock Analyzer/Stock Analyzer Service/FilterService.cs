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
    public FilterService(IFilterRepository filterRepository)
    {
      _filterRepository = filterRepository;
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
      /*_filterRepository.StoreFilterResultForAllCriterias(filterDate);
      return null;*/

      /*var criterias = _filterRepository.GetFilterCriterias(filter);

      DateTime? lastEntryDateOfFilterResult = filter.Criterias
        .FirstOrDefault()?.FilterResults
        .OrderByDescending(_ => _.CalculationDate)
        .FirstOrDefault()?.CalculationDate;

      if(lastEntryDateOfFilterResult == null || lastEntryDateOfFilterResult < filterDate)
      {
        throw new InvalidFilterDateException($"Please Enter latest data of {filterDate}");
      }*/

      filter.Criterias.ForEach(crt => FilterResultInCriteria(crt, filterDate));
      var filterdValues = GetValuesBasedOnAllCriterias(filter.Criterias);

      return filterdValues;
    }

    private static List<FilterResult> GetValuesBasedOnAllCriterias(List<FilterCriteria> criterias)
    {
      var filteredValues = new List<FilterResult>();
      for (int i = 0; i < criterias.Count(); i++)
      {
        if (i > 0 && criterias[i - 1].LogicalOperator == LogicalOperator.And)
        {
          filteredValues = filteredValues
              .Join(criterias[i].FilterResults,
                obj1 => obj1.Company.Id, obj2 => obj2.Company.Id,
                (obj1, obj2) => new List<FilterResult> { obj1, obj2 })
              .SelectMany(_ => _)
              .ToList();
        }
        else
        {
          filteredValues.AddRange(criterias[i].FilterResults);
        }
      }
      return filteredValues;
    }

    private void FilterResultInCriteria(FilterCriteria criteria, DateTime filterDate)
    {
      DateTime fromDate = filterDate.AddBusinessDays(-2);

      var filterResults = _filterRepository.GetFilterResults(criteria, fromDate, filterDate);

      var filterResultsGroupByCompany = from filterResult in filterResults
                                        group filterResult by filterResult.Company.Id into eGroup
                                        orderby eGroup.Key descending
                                        where eGroup.Count() >= 2
                                        select new
                                        {
                                          Key = eGroup.Key,
                                          FilterResults = eGroup
                                            .OrderByDescending(_ => _.CalculationDate)
                                            .ToList()
                                        };

      criteria.FilterResults = filterResultsGroupByCompany
        .Where(_ => (criteria.ChangeType == ChangeType.Increase
                        && _.FilterResults[0].Value > _.FilterResults[1].Value)
                    || (criteria.ChangeType == ChangeType.Decrease
                        && _.FilterResults[0].Value < _.FilterResults[1].Value))
        .Select(_ => _.FilterResults[0])
        .ToList();
    }
  }
}
