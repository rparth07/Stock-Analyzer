using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Repository
{
  public class CompanyRepository : ICompanyRepository, IDisposable
  {
    private readonly StockAnalyzerContext _context;
    private readonly IMapper _mapper;

    public CompanyRepository(StockAnalyzerContext context, IMapper mapper)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public void AddCompanies(List<Company> companiesToInsert)
    {
      var companyInfos = _mapper.Map<List<CompanyDataModel>>(companiesToInsert);

      _context.Company.AddRange(companyInfos);
      try
      {
        _context.SaveChanges();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message.ToString());
      }
    }

    public List<Company> GetAllCompaniesWithAllInfo()
    {
      var companies = _context.Company
          .Include("BulkDeals")
          .Include("BhavCopyInfos")
          .AsNoTracking()
          .ToList()
          .OrderBy(_ => _.BulkDeals.OrderByDescending(_ => _.DealDate));

      return _mapper.Map<List<Company>>(companies);
    }

    public Company GetCompanyByName(string companyName)
    {
      var company = _context.Company
          .Include("BulkDeals")
          .Include("BhavCopyInfos")
          .Include("BulkDeals.Client")
          .AsNoTracking()
          .FirstOrDefault(_ => _.Symbol == companyName);

      return _mapper.Map<Company>(company);
    }

    public List<Company> GetAllCompanies()
    {
      var companies = _context.Company
          .OrderBy(_ => _.Symbol)
          .ToList();

      return _mapper.Map<List<Company>>(companies);
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
