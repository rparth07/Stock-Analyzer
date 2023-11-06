using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Repository
{
  public class BulkDealRepository : IBulkDealRepository, IDisposable
  {
    private readonly StockAnalyzerContext _context;
    private readonly IMapper _mapper;

    public BulkDealRepository(StockAnalyzerContext context, IMapper mapper)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public List<BulkDeal> GetBulkDealsByCompany(string company)
    {
      var bulkDeals = _context.BulkDeal
          .Where(_ => _.Company.Symbol.Equals(company))
          .Include("Company")
          .Include("Client")
          .AsNoTracking()
          .ToList();

      return _mapper.Map<List<BulkDeal>>(bulkDeals);
    }

    public List<BhavCopyInfo> GetAllBhavInfosWithCompanies()
    {
      var bhavInfoWithCompany = _context.BhavCopyInfo
          .Include("Company")
          .AsNoTracking()
          .ToList();

      return _mapper.Map<List<BhavCopyInfo>>(bhavInfoWithCompany);
    }

    public List<BhavCopyInfo> GetAllBhavInfos(DateTime filterDate)
    {
      var bhavInfoWithCompany = _context.BhavCopyInfo
          .Include("Company")
          .AsNoTracking()
          .Where(_ => _.Date.Date.Equals(filterDate.Date))
          .ToList();

      return _mapper.Map<List<BhavCopyInfo>>(bhavInfoWithCompany);
    }

    public void AddBulkDeals(List<BulkDeal> bulkDealsToInsert)
    {
      var bulkDealInfos = _mapper.Map<List<BulkDealDataModel>>(bulkDealsToInsert);

      bulkDealInfos
          .ForEach(_ =>
          {
            _.Company = _context.Company
                      .Where(company => company.Symbol.Equals(_.Company.Symbol))
                      .First();
            _.Client = _context.Client
                      .Where(client => client.Name.Equals(_.Client.Name))
                      .First();
          });

      _context.BulkDeal.AddRange(bulkDealInfos);
      _context.SaveChanges();
    }

    public List<BulkDeal> GetAllBulkDeals(DateTime filterDate)
    {
      var bulkDeals = _context.BulkDeal
          .Include("Company")
          .Include("Client")
          .AsNoTracking()
          .Where(_ => _.DealDate.Date.Equals(filterDate.Date))
          .ToList();

      return _mapper.Map<List<BulkDeal>>(bulkDeals);
    }
    public List<BulkDeal> GetAllBulkDeals()
    {
      var bulkDeals = _context.BulkDeal
          .Include("Company")
          .Include("Client")
          .AsNoTracking()
          .ToList();

      return _mapper.Map<List<BulkDeal>>(bulkDeals);
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
