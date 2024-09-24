using FluentDateTime;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using System.Text.RegularExpressions;

namespace Stock_Analyzer_Service.FilterCalculationType
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

      var filterCriteriaResults = new List<FilterResult>();//Create dictionary by company name to use it for improvement.

      var bhavInfoOfTodayByCompany = GetBhavInfosByCompany(calculationDate, periodValue, series);
      var bhavInfoOfPreviousDayByCompany = GetBhavInfosByCompany(calculationDate.AddBusinessDays(-1), periodValue, series);

      foreach (var company in companies)
      {
        if (Regex.Matches(company.Symbol, "\\d{3}GS\\d{4}").Count > 0)
        {
          continue;
        }
        EnsureAllFieldsPresents(fieldName, periodValue, calculationDate, company.Symbol, series);

        bhavInfoOfTodayByCompany.TryGetValue(company.Symbol, out List<BhavCopyInfo> bhavCopyInfosOfToday);
        bhavInfoOfPreviousDayByCompany.TryGetValue(company.Symbol, out List<BhavCopyInfo> bhavCopyInfosOfPreviousDay);
        if (bhavInfoOfTodayByCompany?.Count > 0 && bhavInfoOfPreviousDayByCompany?.Count > 0)
        {
          var valueOnCalculationDate = GetDataOfField(fieldName, bhavCopyInfosOfToday);
          var valueOnPreviousDate = GetDataOfField(fieldName, bhavCopyInfosOfPreviousDay);

          var matchCriteria = DoesMatchCriteria(filterCriteria, valueOnPreviousDate, valueOnCalculationDate);

          if (matchCriteria)
            filterCriteriaResults.Add(CreateFilterResult(filterCriteria, company, valueOnCalculationDate, calculationDate));
        }
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

    private static double GetDataOfField(string fieldName, List<BhavCopyInfo> bhavInfos)
    {
      var property = typeof(BhavCopyInfo).GetProperty(fieldName);

      if (property != null && property.PropertyType == typeof(double))
      {
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

    private Dictionary<string, List<BhavCopyInfo>> GetBhavInfosByCompany(DateTime calculationDate,
                                                int periodValue,
                                                string series)
    {
      var fromDate = calculationDate.AddBusinessDays(-periodValue);
      var bhavInfos = _bhavInfoRepository.GetBhvaInfosBy(fromDate, calculationDate, series);

      var bhavInfosbyCompany = bhavInfos
        .GroupBy(b => b.Company.Symbol)
        .ToDictionary(g => g.Key, g => g.ToList());

      return bhavInfosbyCompany;
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
