using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Repository.Repository
{
  public class BhavInfoRepository : IBhavInfoRepository, IDisposable
  {

    private readonly StockAnalyzerContext _context;
    private readonly IMapper _mapper;

    public BhavInfoRepository(StockAnalyzerContext context, IMapper mapper)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public void AddBhavInfos(List<BhavCopyInfo> bhavCopyInfosToInsert)
    {
      var bhavCopyInfos = _mapper.Map<List<BhavCopyInfoDataModel>>(bhavCopyInfosToInsert);
      var companies = _context.Company.ToList();

      bhavCopyInfos
          .ForEach(_ => _.Company = companies
              .First(company => company.Symbol.Equals(_.Company.Symbol)));

      _context.BhavCopyInfo.AddRange(bhavCopyInfos);

      try
      {
        _context.SaveChanges();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message.ToString());
      }
    }

    public List<BhavCopyInfo> GetAllBhavInfosWithCompany(DateTime date)
    {
      var bhavInfoWithCompany = _context.BhavCopyInfo
          .Include("Company")
          .Where(_ => _.Date.Equals(date))
          .AsNoTracking()
          .ToList();

      return _mapper.Map<List<BhavCopyInfo>>(bhavInfoWithCompany);
    }

    public List<BhavCopyInfo> GetBhavInfosByCompany(string company)
    {
      var bhavInfoWithCompany = _context.BhavCopyInfo
          .Where(_ => _.Company.Symbol.Equals(company))
          .Include("Company")
          .AsNoTracking()
          .ToList();

      return _mapper.Map<List<BhavCopyInfo>>(bhavInfoWithCompany);
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

    public List<BhavCopyInfo> GetBhavInfosBetween(DateTime startDate, DateTime endDate)
    {
      var bhavInfoWithCompany = _context.BhavCopyInfo
          .Include("Company")
          .AsNoTracking()
          .Where(_ => _.Date >= startDate && _.Date <= endDate)
          .ToList();

      return _mapper.Map<List<BhavCopyInfo>>(bhavInfoWithCompany);
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

    public List<BhavCopyInfo> GetBhvaInfosBy(DateTime fromDate, DateTime toDate, string series, string companyName)
    {
      var bhavInfos = _context.BhavCopyInfo
            .Where(bc => bc.Date > fromDate && bc.Date <= toDate
              && (bc.Series.ToLower() == series.ToLower())
              && bc.Company.Symbol == companyName)
            .AsNoTracking()
            .ToList();

      return _mapper.Map<List<BhavCopyInfo>>(bhavInfos);
    }

    public DateTime GetLatestBhavInfoDate()
    {
      return _context.BhavCopyInfo
        .OrderByDescending(bc => bc.Date)
        .FirstOrDefault()
        .Date;
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
