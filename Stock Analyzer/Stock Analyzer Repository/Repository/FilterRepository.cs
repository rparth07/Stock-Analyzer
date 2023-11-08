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
  public class FilterRepository : IFilterRepository, IDisposable
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
        .AsNoTracking()
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
        .AsNoTracking()
        .ToList();

      return _mapper.Map<List<FilterResult>>(filterResults);
    }

    public void AddFilterResults(List<FilterResult> filterResults)
    {
      var filterResultstoInsert = _mapper.Map<List<FilterResultDataModel>>(filterResults);

      filterResultstoInsert.ForEach(_ =>
      {
        _.Company = _context.Company
              .First(company => company.Symbol.Equals(_.Company.Symbol));

        _.FilterCriteria = _context.FilterCriteria
              .First(fc => fc.Id.Equals(_.FilterCriteria.Id));
      });

      _context.FilterResult.AddRange(filterResultstoInsert);
      _context.SaveChanges();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose resources when needed
      }
    }
  }
}
