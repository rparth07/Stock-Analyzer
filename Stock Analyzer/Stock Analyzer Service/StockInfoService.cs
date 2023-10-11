using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Service.Interface;
using System.Linq;

namespace Stock_Analyzer_Service
{
    public class StockInfoService : IStockInfoService
    {
        private readonly IStockInfoRepository _stockInfoRepository;
        public StockInfoService(IStockInfoRepository stockInfoRepository)
        {
            _stockInfoRepository = stockInfoRepository ?? throw new ArgumentNullException(nameof(stockInfoRepository));
        }

        public void AddCompanies(List<Company> companies)
        {
            if (companies != null && companies.Count > 0)
            {
                List<Company> companiesFromServer = GetAllCompaniesWithAllInfo();
                var companiesToInsert = GetCompaniesToInsert(companies, companiesFromServer);
                if(companiesToInsert.Count > 0)
                {
                    _stockInfoRepository.AddCompanies(companiesToInsert);
                }
            }
        }

        private List<Company> GetCompaniesToInsert(List<Company> companies, List<Company> companiesFromServer)
        {
            companies = companies.DistinctBy(_ => _.Symbol).ToList();
            if(companiesFromServer.Count == 0)
            {
                return companies;
            }

            List<Company> companiesToInsert = companies
                .Where(_ => companiesFromServer
                    .FirstOrDefault(cs => cs.Symbol.Equals(_.Symbol)) == null).ToList();

            return companiesToInsert;
        }

        public void AddBhavInfos(List<BhavCopyInfo> bhavInfos)
        {
            if (bhavInfos != null && bhavInfos.Count > 0)
            {
                DateTime date = bhavInfos.First().Date;
                List<BhavCopyInfo> bhavInfoFromServer = GetAllBhavInfosWithCompany(date);
                var bhavInfosToInsert = GetBhavInfosToInsert(bhavInfos, bhavInfoFromServer);
                if(bhavInfosToInsert.Count > 0)
                {
                    _stockInfoRepository.AddBhavInfos(bhavInfosToInsert);
                }
            }
        }

        private List<BhavCopyInfo> GetBhavInfosToInsert(List<BhavCopyInfo> bhavInfos, List<BhavCopyInfo> bhavInfoFromServer)
        {
            if (bhavInfoFromServer.Count == 0)
            {
                return bhavInfos;
            }

            List<BhavCopyInfo> bhavInfosToInsert = bhavInfos
                .Where(_ => bhavInfoFromServer
                    .FirstOrDefault(bis => bis.Company.Symbol.Equals(_.Company.Symbol)
                        && bis.Series.Equals(_.Series)
                        && bis.Date.Equals(_.Date)) == null)
                .ToList();

            return bhavInfosToInsert;
        }

        public List<Company> GetAllCompaniesWithAllInfo()
        {
            var companies = _stockInfoRepository.GetAllCompaniesWithAllInfo();
            companies.ForEach(x => x.BulkDeals = x.BulkDeals.OrderByDescending(_ => _.DealDate).ToList());
            return companies;
        }

        public Company GetCompanyByName(string companyName)
        {
            var company = _stockInfoRepository.GetCompanyByName(companyName);

            return company;
        }

        public List<Company> GetAllCompanies()
        {
            var companies = _stockInfoRepository.GetAllCompanies();
            return companies;
        }

        public List<BhavCopyInfo> GetAllBhavInfosWithCompany(DateTime date)
        {
            return _stockInfoRepository.GetAllBhavInfosWithCompany(date);
        }

        public List<BhavCopyInfo> GetAllBhavInfosWithCompanies()
        {
            return _stockInfoRepository.GetAllBhavInfosWithCompanies();
        }

        public List<BhavCopyInfo> GetBhavInfosByCompany(string company)
        {
            return _stockInfoRepository.GetBhavInfosByCompany(company);
        }

        public List<BulkDeal> GetBulkDealsByCompany(string company)
        {
            return _stockInfoRepository.GetBulkDealsByCompany(company);
        }

        public void AddClients(List<Client> clients)
        {
            if (clients != null && clients.Count > 0)
            {
                List<Client> clientsFromServer = GetAllClients();
                var clientsToInsert = GetClientsToInsert(clients, clientsFromServer);
                if (clientsToInsert.Count > 0)
                {
                    _stockInfoRepository.AddClients(clientsToInsert);
                }
            }
        }

        public List<Client> GetAllClients()
        {
            var clients =  _stockInfoRepository.GetAllClients();
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
                    _stockInfoRepository.AddBulkDeals(bulkDealsToInsert);
                }
            }
        }

        public List<BulkDeal> GetAllBulkDeals()
        {
            return _stockInfoRepository.GetAllBulkDeals();
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