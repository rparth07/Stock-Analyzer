using AutoMapper;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Repository.DataModels;
using Stock_Analyzer_Repository.DataModels.Filter;
using Microsoft.EntityFrameworkCore;
using PeriodTypeModel = Stock_Analyzer_Repository.DataModels.Filter.PeriodType;

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
        .Include("Criterias.FilterResults")
        .Include("Criterias.FilterResults.CompanyDataModel")
        .AsNoTracking()
        .First(_ => _.FilterName.Equals(filterName));

      return _mapper.Map<Filter>(filter);
    }

    public List<FilterCriteria> GetFilterCriterias(Filter filter)
    {
      var filterCriteria = _context.FilterCriteria
        .Include("FilterResults")
        .Include("FilterResults.CompanyDataModel")
        .AsNoTracking()
        .Where(_ => _.FilterDataModel.Id.Equals(filter.Id));

      return _mapper.Map<List<FilterCriteria>>(filterCriteria);
    }

    public void StoreFilterResultForAllCriterias(DateTime calculationDate)
    {
      var filterCriterias = _context.FilterCriteria.ToList();
      var companies = _context.Company.ToList();

      List<FilterResultDataModel> filterResults = new List<FilterResultDataModel>();

      foreach(var criteria in filterCriterias)
      {
        var fieldName = criteria.FieldName;
        var periodValue = ConvertToDays(criteria.PeriodType, criteria.PeriodValue);

        foreach(var company in companies)
        {
          var fieldValue = GetDataOfField(fieldName, periodValue, calculationDate, company.Symbol);
          var filterResult = CreateFilterResult(criteria, company, fieldValue, calculationDate);

          filterResults.Add(filterResult);
        }
      }

      InsertFilterResult(filterResults);
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
        FilterCriteriaDataModel = criteria,
        CalculationDate = calculationDate,
        CompanyDataModel = company,
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
                                  string companyName)
    {
      /*_context.BhavCopyInfo
        .Where(_ => _.Date <= calculationDate && _.Date >= calculationDate.AddDays(periodValue))
        .Average(_ => _.ClosePrice);*/

      var property = typeof(BhavCopyInfo).GetProperty(fieldName);

      if (property != null && property.PropertyType == typeof(double))
      {
        var fromDate = calculationDate.AddDays(-periodValue);

        var average = _context.BhavCopyInfo
            .Where(bc => bc.Date >= fromDate && bc.Date <= calculationDate
              && (bc.Series == "EQ" || bc.Series == "BE")
              && bc.Company.Symbol == companyName)
            .Average(bc => (double)property.GetValue(bc));

        return average;
      }
      else
      {
        // Handle the case where the fieldName doesn't exist or isn't a double property
        throw new Exception("Invalid Field to filter"); // You can return an appropriate default value or handle the error as needed
      }
    }
  }
}
