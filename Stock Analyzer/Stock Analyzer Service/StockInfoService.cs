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
      var bhavInfosToInsert = _bhavInfoRepository.GetBhavInfosToInsert(bhavInfos);

      if (bhavInfosToInsert.Count > 0)
      {
        _bhavInfoRepository.AddBhavInfos(bhavInfosToInsert);
        _filterService.StoreFilterResultsByFilterFor(calculationDate);
      }
    }

    //Need to use
    public List<BhavCopyInfo> GetAllBhavInfosWithCompany(DateTime date)
    {
      return _bhavInfoRepository.GetAllBhavInfosWithCompany(date);
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
      var companiesToInsert = _companyRepository.GetCompaniesToInsert(companies);
      if (companiesToInsert.Count > 0)
      {
        _companyRepository.AddCompanies(companiesToInsert);
      }
    }

    public List<Company> GetAllCompaniesWithAllInfo()
    {
      var companies = _companyRepository.GetAllCompaniesWithAllInfo();
      companies.ForEach(x => x.BulkDeals = x.BulkDeals.OrderByDescending(_ => _.DealDate).ToList());
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
      List<Client> clientsFromServer = GetAllClients();
      var clientsToInsert = GetClientsToInsert(clients, clientsFromServer);
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

    private List<Client> GetClientsToInsert(List<Client> clients, List<Client> clientsFromServer)
    {
      clients = clients.DistinctBy(_ => _.Name).ToList();
      if (clientsFromServer.Count == 0)
      {
        return clients;
      }

      List<Client> clientsToInsert = clients
          .Where(_ => clientsFromServer
              .FirstOrDefault(cs => cs.Name.Equals(_.Name)) == null)
          .ToList();

      return clientsToInsert;
    }

    public void AddBulkDeals(List<BulkDeal> bulkDeals)
    {
      if (bulkDeals != null && bulkDeals.Count > 0)
      {
        List<BulkDeal> bulkDealsFromServer = GetAllBulkDeals();
        var bulkDealsToInsert = GetBulkDealsToInsert(bulkDeals, bulkDealsFromServer);
        if (bulkDealsToInsert.Count > 0)
        {
          _bulkDealRepository.AddBulkDeals(bulkDealsToInsert);
        }
      }
    }

    public List<BulkDeal> GetBulkDealsByCompany(string company)
    {
      return _bulkDealRepository.GetBulkDealsByCompany(company);
    }

    public List<BulkDeal> GetAllBulkDeals()
    {
      return _bulkDealRepository.GetAllBulkDeals();
    }

    private List<BulkDeal> GetBulkDealsToInsert(List<BulkDeal> bulkDeals, List<BulkDeal> bulkDealsFromServer)
    {
      if (bulkDealsFromServer.Count == 0)
      {
        return bulkDeals;
      }

      List<BulkDeal> bulkDealsToInsert = bulkDeals
          .Where(_ => bulkDealsFromServer
              .FirstOrDefault(cs => cs.DealDate.Equals(_.DealDate)) == null)
          .ToList();

      return bulkDealsToInsert;
    }

  }
}
