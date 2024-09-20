using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Service.Interface;
using System.Linq;

namespace Stock_Analyzer_Service
{
  public class StockInfoService : IStockInfoService
  {
    private readonly IBhavInfoRepository _bhavInfoRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IBulkDealRepository _bulkDealRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IFilterService _filterService;
    public StockInfoService(IBhavInfoRepository bhavInfoRepository,
                            ICompanyRepository companyRepository,
                            IBulkDealRepository bulkDealRepository,
                            IClientRepository clientRepository,
                            IFilterService filterService)
    {
      _bhavInfoRepository = bhavInfoRepository ?? throw new ArgumentNullException(nameof(bhavInfoRepository));
      _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
      _bulkDealRepository = bulkDealRepository ?? throw new ArgumentNullException(nameof(bulkDealRepository));
      _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
      _filterService = filterService ?? throw new ArgumentNullException(nameof(filterService));
    }

    public void AddBhavInfos(List<BhavCopyInfo> bhavInfos)
    {
      if (bhavInfos == null || bhavInfos.Count == 0)
      {
        return;
      }
      DateTime calculationDate = bhavInfos.First().Date;
      var existingBhavCopyInfos = _bhavInfoRepository.GetAllBhavInfosWithCompany(calculationDate);

      bhavInfos.RemoveAll(_ => existingBhavCopyInfos
          .Any(eb => eb.Company.Symbol.Equals(_.Company.Symbol)
                && eb.Series.Equals(_.Series)
                && eb.Date.Equals(_.Date)));
      if (bhavInfos.Count > 0)
      {
        _bhavInfoRepository.AddBhavInfos(bhavInfos);
        _filterService.StoreFilterResultsByFilterFor(calculationDate);
      }
    }

    //Need to use
    public List<BhavCopyInfo> GetAllBhavInfosWithCompany(DateTime date)
    {
      return _bhavInfoRepository.GetAllBhavInfosWithCompany(date);
    }

    public List<BhavCopyInfo> GetBhavInfosBetween(DateTime startDate, DateTime endDate)
    {
      return _bhavInfoRepository.GetBhavInfosBetween(startDate, endDate);
    }

    public List<BhavCopyInfo> GetAllBhavInfosWithCompanies()
    {
      return _bhavInfoRepository.GetAllBhavInfosWithCompanies();
    }

    public List<BhavCopyInfo> GetBhavInfosByCompany(string company)
    {
      return _bhavInfoRepository.GetBhavInfosByCompany(company);
    }


    public void AddCompanies(List<Company> companies)
    {
      if (companies == null || companies.Count == 0)
      {
        return;
      }

      var existingCompanies = _companyRepository.GetAllCompanies();
      companies.RemoveAll(_ => existingCompanies
            .Any(ec => ec.Symbol.Equals(_.Symbol)));
      var companiesToInsert = companies.DistinctBy(_ => _.Symbol).ToList();

      if (companiesToInsert.Count > 0)
      {
        _companyRepository.AddCompanies(companiesToInsert);
      }
    }

    public List<Company> GetAllCompaniesWithAllInfo()
    {
      var companies = _companyRepository.GetAllCompaniesWithAllInfo();
      //companies.ForEach(x => x.BulkDeals = x.BulkDeals.OrderByDescending(_ => _.DealDate).ToList());
      return companies;
    }

    public Company GetCompanyByName(string companyName)
    {
      var company = _companyRepository.GetCompanyByName(companyName);

      return company;
    }

    public List<Company> GetAllCompanies()
    {
      var companies = _companyRepository.GetAllCompanies();
      return companies;
    }


    public void AddClients(List<Client> clients)
    {
      if (clients == null || clients.Count == 0)
      {
        return;
      }

      List<Client> existingClients = GetAllClients();
      clients.RemoveAll(_ => existingClients.Any(ec => ec.Name.Equals(_.Name)));
      var clientsToInsert = clients.DistinctBy(_ => _.Name).ToList();

      if (clientsToInsert.Count > 0)
      {
        _clientRepository.AddClients(clientsToInsert);
      }
    }

    public List<Client> GetAllClients()
    {
      var clients = _clientRepository.GetAllClients();
      clients.ForEach(x => x.Deals = x.Deals.OrderByDescending(_ => _.DealDate).ToList());
      return clients;
    }

    public void AddBulkDeals(List<BulkDeal> bulkDeals)
    {
      if (bulkDeals != null && bulkDeals.Count > 0)
      {
        List<BulkDeal> existingBulkDeals = GetAllBulkDeals();
        bulkDeals.RemoveAll(_ => existingBulkDeals
             .Any(eb => eb.DealDate.Equals(_.DealDate)
                  && eb.Company.Symbol.Equals(_.Company.Symbol)
                  && eb.Client.Name.Equals(_.Client.Name)));

        if (bulkDeals.Count > 0)
        {
          _bulkDealRepository.AddBulkDeals(bulkDeals);
        }
      }
    }

    public List<BulkDeal> GetBulkDealsByCompany(string company)
    {
      return _bulkDealRepository.GetBulkDealsByCompany(company);
    }

    public List<BulkDeal> GetBulkDealsBetween(DateTime startDate, DateTime endDate)
    {
      return _bulkDealRepository.GetBulkDealsBetween(startDate, endDate);
    }

    public List<BulkDeal> GetAllBulkDeals()
    {
      return _bulkDealRepository.GetAllBulkDeals();
    }
  }
}
