using FluentDateTime;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stock_Analyzer_Service.FilterCalculationType
{
  public class ContinousAverage
  {

    private IBhavInfoRepository _bhavInfoRepository;

    public ContinousAverage(IBhavInfoRepository bhavInfoRepository)
    {
      _bhavInfoRepository = bhavInfoRepository;
    }

    public List<FilterResult> ExecuteMovingAverageFilter(Filter filter, DateTime calculationDate, List<Company> companies)
    {
      var filterCriterias = filter.Criterias;

      var filterResults = new List<FilterResult>();

      for (int i = 0; i < filterCriterias.Count(); i++)
      {
        var filterCriteriaResults = ExecuteFilterCriteria(filterCriterias[i], calculationDate, companies);

        if (i > 0 && filterCriterias[i - 1].LogicalOperator == LogicalOperator.And)
        {
          filterResults = filterResults
              .Join(filterCriteriaResults,
                obj1 => obj1.Company.Id, obj2 => obj2.Company.Id,
                (obj1, obj2) => new List<FilterResult> { obj1, obj2 })
              .SelectMany(_ => _)
              .ToList();
        }
        else
        {
          filterResults.AddRange(filterCriteriaResults);
        }
      }

      return filterResults;
    }

    private List<FilterResult> ExecuteFilterCriteria(FilterCriteria filterCriteria, DateTime calculationDate, List<Company> companies)
    {
      var fieldName = filterCriteria.FieldName;
      var series = filterCriteria.Filter.Series;
      var periodValue = ConvertToDays(filterCriteria.PeriodType, filterCriteria.PeriodValue);

      var filterCriteriaResults = new List<FilterResult>();

      foreach (var company in companies)
      {
        if (Regex.Matches(company.Symbol, "\\d{3}GS\\d{4}").Count > 0)
        {
          continue;
        }

        EnsureAllFieldsPresents(fieldName, calculationDate, periodValue, company.Symbol, series);

        var bhavInfos = GetBhavInfosBy(calculationDate, periodValue, company.Symbol, series);

        var matchCriteria = DoesMatchCriteria(filterCriteria, bhavInfos, fieldName, periodValue);

        var average = GetAverageOf(fieldName, calculationDate, periodValue, bhavInfos);

        if (matchCriteria)
          filterCriteriaResults.Add(CreateFilterResult(filterCriteria, company, average, calculationDate));
      }

      return filterCriteriaResults;
    }

    private static bool DoesMatchCriteria(FilterCriteria criteria,
                                          List<BhavCopyInfo> bhavInfos,
                                          string fieldName,
                                          int periodValue)
    {
      if(criteria == null || bhavInfos.Count == 0 || bhavInfos.Count() < periodValue)
      {
        return false;
      }

      var property = typeof(BhavCopyInfo).GetProperty(fieldName);

      if (property != null && property.PropertyType == typeof(double))
      {
        bhavInfos = bhavInfos
                      .OrderBy(_ => _.Date)
                      .ToList();
        //TODO: Improve this logic using for loop and break it and move on to the next as soon as
        // find that the elements does not satisfy the condition
        //Then, try to improve it using linq
        if (criteria.ChangeType == ChangeType.Increase)
        {
          return bhavInfos
            .Zip(bhavInfos.Skip(1),
                  (current, next) => (double)property.GetValue(current) <= (double)property.GetValue(next))
            .All(isIncreasing => isIncreasing);
        } else
        {
          return bhavInfos
            .Zip(bhavInfos.Skip(1),
                  (current, next) => (double)property.GetValue(current) >= (double)property.GetValue(next))
            .All(isIncreasing => isIncreasing);
        }

        /*(double)property.GetValue(bc)*/
      }
      else
      {
        // Handle the case where the fieldName doesn't exist or isn't a double property
        throw new Exception("Invalid Field to filter"); // You can return an appropriate default value or handle the error as needed
      }
    }

    private static double GetAverageOf(string fieldName, DateTime calculationDate, int periodValue, List<BhavCopyInfo> bhavInfos)
    {
      var property = typeof(BhavCopyInfo).GetProperty(fieldName);

      if (property != null && property.PropertyType == typeof(double))
      {
        var fromDate = calculationDate.AddBusinessDays(-periodValue);

        var average = bhavInfos.Count() == 0 ? 0.0
          : bhavInfos
              .Average(bc => (double)property.GetValue(bc));

        return average;
      }
      else
      {
        // Handle the case where the fieldName doesn't exist or isn't a double property
        throw new Exception("Invalid Field to filter"); // You can return an appropriate default value or handle the error as needed
      }
    }

    private static FilterResult CreateFilterResult(FilterCriteria criteria,
                                    Company company,
                                    double fieldValue,
                                    DateTime calculationDate)
    {
      return new FilterResult()
      {
        FilterCriteria = criteria,
        CalculationDate = calculationDate,
        Company = company,
        Value = fieldValue
      };
    }

    private static int ConvertToDays(PeriodType periodType, int periodValue)
    {
      if (PeriodType.Weeks == periodType)
      {
        return periodValue * 7;
      }
      else if (PeriodType.Months == periodType)
      {
        return periodValue * 30;
      }
      if (PeriodType.Years == periodType)
      {
        return periodValue * 365;
      }
      return periodValue;
    }

    private List<BhavCopyInfo> GetBhavInfosBy(DateTime calculationDate,
                                              int periodValue,
                                              string companyName,
                                              string series)
    {
      var fromDate = calculationDate.AddBusinessDays(-periodValue);
      return _bhavInfoRepository.GetBhvaInfosBy(fromDate, calculationDate, series, companyName);
    }

    private static void EnsureAllFieldsPresents(string fieldName, DateTime? calculationDate, int? periodValue, string companyName, string series)
    {
      if (fieldName == null || periodValue == null
        || calculationDate == null || companyName == null
        || series == null)
      {
        throw new Exception("Invalid Filter, Please Enter data correctly!");
      }
    }

  }
}
