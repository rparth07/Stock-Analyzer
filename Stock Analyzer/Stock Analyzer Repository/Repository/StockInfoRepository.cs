using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Repository
{
  public class StockInfoRepository : IStockInfoRepository, IDisposable
    {
        private readonly StockAnalyzerContext _context;
        private readonly IMapper _mapper;

        public StockInfoRepository(StockAnalyzerContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void AddCompanies(List<Company> companiesToInsert)
        {
            var companyInfos = _mapper.Map<List<CompanyDataModel>>(companiesToInsert);

            _context.Company.AddRange(companyInfos);
            _context.SaveChanges();
        }

        public List<Company> GetAllCompaniesWithAllInfo()
        {
            var companies = _context.Company
                .Include("BulkDeals")
                .Include("BhavCopyInfos")
                .AsNoTracking()
                .ToList();

            return _mapper.Map<List<Company>>(companies);
        }

        public Company GetCompanyByName(string companyName)
        {
            var company = _context.Company
                .Include("BulkDeals")
                .Include("BhavCopyInfos")
                .Include("BulkDeals.Client")
                .FirstOrDefault(_ => _.Symbol == companyName);

            return _mapper.Map<Company>(company);
        }

        public List<Company> GetAllCompanies()
        {
            var companies = _context.Company
                .OrderBy(_ => _.Symbol)
                .AsNoTracking()
                .ToList();

            return _mapper.Map<List<Company>>(companies);
        }

        public void AddClients(List<Client> clientsToInsert)
        {
            var clientInfos = _mapper.Map<List<ClientDataModel>>(clientsToInsert);

            _context.Client.AddRange(clientInfos);
            _context.SaveChanges();
        }

        public List<Client> GetAllClients()
        {
            var clients = _context.Client
                .Include("Deals")
                .Include("Deals.Company")
                .ToList();

            return _mapper.Map<List<Client>>(clients);
        }

        public void AddBhavInfos(List<BhavCopyInfo> bhavCopyInfosToInsert)
        {
            var bhavCopyInfos = _mapper.Map<List<BhavCopyInfoDataModel>>(bhavCopyInfosToInsert);

            bhavCopyInfos
                .ForEach(_ => _.Company = _context.Company
                    .Where(company => company.Symbol.Equals(_.Company.Symbol))
                    .First());

            _context.BhavCopyInfo.AddRange(bhavCopyInfos);
            _context.SaveChanges();
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

        public List<BhavCopyInfo> GetAllBhavInfosWithCompanies()
        {
            var bhavInfoWithCompany = _context.BhavCopyInfo
                .Include("Company")
                .AsNoTracking()
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
