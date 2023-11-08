using FluentDateTime;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stock_Analyzer_Service.CalculationType
{
  public class MovingAverage
  {
    private IBhavInfoRepository _bhavInfoRepository;

    public MovingAverage(IBhavInfoRepository bhavInfoRepository)
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

        var valueOnCalculationDate = GetDataOfField(fieldName, periodValue, calculationDate, company.Symbol, series);
        var valueOnPreviousDate = GetDataOfField(fieldName, periodValue, calculationDate.AddBusinessDays(-1), company.Symbol, series);

        var matchCriteria = DoesMatchCriteria(filterCriteria, valueOnPreviousDate, valueOnCalculationDate);

        if (matchCriteria)
          filterCriteriaResults.Add(CreateFilterResult(filterCriteria, company, valueOnCalculationDate, calculationDate));
      }

      return filterCriteriaResults;
    }

    private static bool DoesMatchCriteria(FilterCriteria criteria, double valueOnPreviousDate, double valueOnCalculationDate)
    {
      if (valueOnPreviousDate == 0 || valueOnCalculationDate == 0)
      {
        return false;
      }

      return criteria.ChangeType == ChangeType.Increase
        ? valueOnPreviousDate < valueOnCalculationDate
        : valueOnPreviousDate > valueOnCalculationDate;
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

    private double GetDataOfField(string fieldName,
                                  int periodValue,
                                  DateTime calculationDate,
                                  string companyName,
                                  string series)
    {
      EnsureAllFieldsPresents(fieldName, periodValue, calculationDate, companyName, series);

      var property = typeof(BhavCopyInfo).GetProperty(fieldName);

      if (property != null && property.PropertyType == typeof(double))
      {
        var fromDate = calculationDate.AddBusinessDays(-periodValue);

        var bhavInfos = _bhavInfoRepository.GetBhvaInfosBy(fromDate, calculationDate, series, companyName);

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

    private static void EnsureAllFieldsPresents(string fieldName, int? periodValue, DateTime? calculationDate, string companyName, string series)
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
