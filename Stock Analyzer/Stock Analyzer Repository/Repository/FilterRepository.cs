using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Repository.DataModels.Filter;

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
      var filterCriteriaIds = filter.Criterias.Select(_ => _.Id).ToList();

      var filterResults = _context.FilterResult
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
      var filterResulToInsert = _mapper.Map<List<FilterResultDataModel>>(filterResults);

      var companies = _context.Company.ToList();
      var filterCriteria = _context.FilterCriteria.ToList();

      filterResulToInsert.ForEach(_ =>
      {
        _.Company = companies
              .First(company => company.Symbol.Equals(_.Company.Symbol));

        _.FilterCriteria = filterCriteria
              .First(fc => fc.Id.Equals(_.FilterCriteria.Id));
      });

      _context.FilterResult.AddRange(filterResulToInsert);
      _context.SaveChanges();
    }

    public bool ExistFilterResultsFor(Filter filter, DateTime calculationDate)
    {
      return _context.FilterResult.Any(_ => _.CalculationDate.Equals(calculationDate)
                      && _.FilterCriteria.Filter.Id.Equals(filter.Id));
    }

    public void DeleteFilterByName(string filterName)
    {
      var filterToDelete = _context.Filter
        .First(_ => _.FilterName == filterName);

      _context.Filter.Remove(filterToDelete);
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
