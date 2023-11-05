using AutoMapper;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Repository.DataModels;
using Stock_Analyzer_Repository.DataModels.Filter;
using Microsoft.EntityFrameworkCore;
using PeriodTypeModel = Stock_Analyzer_Repository.DataModels.Filter.PeriodType;
using FluentDateTime;
using System.Text.RegularExpressions;
using ChangeType = Stock_Analyzer_Repository.DataModels.Filter.ChangeType;
using LogicalOperator = Stock_Analyzer_Repository.DataModels.Filter.LogicalOperator;

namespace Stock_Analyzer_Repository.Repository
{
  public class FilterRepository : IFilterRepository
  {
    private readonly StockAnalyzerContext _context;
    private readonly IMapper _mapper;

    public FilterRepository(StockAnalyzerContext context, IMapper mapper)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public void AddFilter(Filter filter)
    {
      var filterData = _mapper.Map<FilterDataModel>(filter);
      _context.Filter.Add(filterData);
      /*
      filterData = _context.Filter
        .First(_ => _.FilterName
          .Equals(filter.FilterName, StringComparison.OrdinalIgnoreCase));

      var filterCriteriaData = filterData.Criterias;

      filterCriteriaData
        .ForEach(_ => _.FilterDataModel = filterData);

      _context.FilterCriteria.AddRange(filterCriteriaData);*/
      _context.SaveChanges();

    }

    public List<Filter> GetFilters()
    {
      var filterData = _context.Filter
        .Include("Criterias")
        .ToList();

      return _mapper.Map<List<Filter>>(filterData);
    }

    public Filter GetFilterByName(string filterName)
    {
      var filter = _context.Filter
        .Include("Criterias")
        .AsNoTracking()
        .First(_ => _.FilterName.Equals(filterName));

      return _mapper.Map<Filter>(filter);
    }

    public List<FilterCriteria> GetFilterCriterias(Filter filter)
    {
      var filterCriteria = _context.FilterCriteria
        .Include("FilterResults")
        .Include("FilterResults.Company")
        .AsNoTracking()
        .Where(_ => _.Filter.Id.Equals(filter.Id))
        .ToList();

      return _mapper.Map<List<FilterCriteria>>(filterCriteria);
    }

    public List<FilterResult> GetFilterResults(Filter filter, DateTime filterDate)
    {
      /*var filterResults = _context.FilterResult
        .AsNoTracking()
        .Where(_ => _.CalculationDate > fromDate
          && _.CalculationDate <= toDate)
        .Select(fr => new FilterResultDataModel
        {
          Id = fr.Id,
          FilterCriteria = fr.FilterCriteria,
          Company = fr.Company,
          CalculationDate = fr.CalculationDate,
          Value = fr.Value,
        })
        .ToList();*/

      var filterCriteriaIds = filter.Criterias.Select(_ => _.Id).ToList();

      var filterResults = _context.FilterResult
        .AsNoTracking()
        .Where(_ => filterCriteriaIds.Contains(_.FilterCriteria.Id)
          && _.CalculationDate.Date == filterDate.Date)
        .Select(fr => new FilterResultDataModel
        {
          Id = fr.Id,
          FilterCriteria = fr.FilterCriteria,
          Company = fr.Company,
          CalculationDate = fr.CalculationDate,
          Value = fr.Value,
        })
        .ToList();

      return _mapper.Map<List<FilterResult>>(filterResults);
    }

    public void StoreFilterResultsByFilterFor(DateTime calculationDate)
    {
      var filters = _context.Filter
        .Include("Criterias")
        .ToList();

      var companies = _context.Company.ToList();

      List<FilterResultDataModel> filterResultsToInsert = new List<FilterResultDataModel>();

      filters.ForEach(filter =>
      {
        var filterResults = ExecuteFilter(filter, calculationDate, companies);
        filterResultsToInsert.AddRange(filterResults);
      });

      InsertFilterResult(filterResultsToInsert);
    }

    private List<FilterResultDataModel> ExecuteFilter(FilterDataModel filter, DateTime calculationDate, List<CompanyDataModel> companies)
    {
      var filterCriterias = filter.Criterias;

      var filterResults = new List<FilterResultDataModel>();

      for (int i = 0; i < filterCriterias.Count(); i++)
      {
        var filterCriteriaResults = ExecuteFilterCriteria(filterCriterias[i], calculationDate, companies);

        if (i > 0 && filterCriterias[i - 1].LogicalOperator == LogicalOperator.And)
        {
          filterResults = filterResults
              .Join(filterCriteriaResults,
                obj1 => obj1.Company.Id, obj2 => obj2.Company.Id,
                (obj1, obj2) => new List<FilterResultDataModel> { obj1, obj2 })
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

    private List<FilterResultDataModel> ExecuteFilterCriteria(FilterCriteriaDataModel filterCriteria, DateTime calculationDate, List<CompanyDataModel> companies)
    {
      var fieldName = filterCriteria.FieldName;
      var series = filterCriteria.Filter.Series;
      var periodValue = ConvertToDays(filterCriteria.PeriodType, filterCriteria.PeriodValue);

      var filterCriteriaResults = new List<FilterResultDataModel>();

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

    private bool DoesMatchCriteria(FilterCriteriaDataModel criteria, double valueOnPreviousDate, double valueOnCalculationDate)
    {
      if(valueOnPreviousDate == 0 || valueOnCalculationDate == 0)
      {
        return false;
      }

      return criteria.ChangeType == ChangeType.Increase
        ? valueOnPreviousDate < valueOnCalculationDate
        : valueOnPreviousDate > valueOnCalculationDate;
    }

    private void InsertFilterResult(List<FilterResultDataModel> filterResults)
    {

      _context.FilterResult.AddRange(filterResults);
      _context.SaveChanges();
    }

    private static FilterResultDataModel CreateFilterResult(FilterCriteriaDataModel criteria,
                                    CompanyDataModel company,
                                    double fieldValue,
                                    DateTime calculationDate)
    {
      return new FilterResultDataModel()
      {
        FilterCriteria = criteria,
        CalculationDate = calculationDate,
        Company = company,
        Value = fieldValue
      };
    }

    private int ConvertToDays(PeriodTypeModel periodType, int periodValue)
    {
      if(PeriodTypeModel.Weeks == periodType)
      {
        return periodValue * 7;
      }
      else if (PeriodTypeModel.Months == periodType)
      {
        return periodValue * 30;
      }
      if (PeriodTypeModel.Years == periodType)
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
      /*_context.BhavCopyInfo
        .Where(_ => _.Date <= calculationDate && _.Date >= calculationDate.AddDays(periodValue))
        .Average(_ => _.ClosePrice);*/
      EnsureAllFieldsPresents(fieldName, periodValue, calculationDate, companyName, series);

      var property = typeof(BhavCopyInfoDataModel).GetProperty(fieldName);

      if (property != null && property.PropertyType == typeof(double))
      {
        var fromDate = calculationDate.AddBusinessDays(-periodValue);

        var bhavInfos = _context.BhavCopyInfo
            .Where(bc => bc.Date > fromDate && bc.Date <= calculationDate
              && (bc.Series.ToLower() == series.ToLower())
              && bc.Company.Symbol == companyName)
            .ToList();

        var average = bhavInfos.Count() == 0 ? 0.0
          : bhavInfos
              .Average(bc =>  (double)property.GetValue(bc));

        return average;
      }
      else
      {
        // Handle the case where the fieldName doesn't exist or isn't a double property
        throw new Exception("Invalid Field to filter"); // You can return an appropriate default value or handle the error as needed
      }
    }

    private void EnsureAllFieldsPresents(string fieldName, int periodValue, DateTime calculationDate, string companyName, string series)
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
