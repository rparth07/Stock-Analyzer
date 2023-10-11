using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer_Service.Interface
{
    public interface IStockInfoService
    {
        public void AddCompanies(List<Company> companies);
        public void AddBhavInfos(List<BhavCopyInfo> bhavInfos);
        public List<Company> GetAllCompaniesWithAllInfo();
        public Company GetCompanyByName(string companyName);
        public List<Company> GetAllCompanies();
        public List<BhavCopyInfo> GetAllBhavInfosWithCompanies();
        public List<BhavCopyInfo> GetBhavInfosByCompany(string company);

        public void AddClients(List<Client> clients);
        public List<Client> GetAllClients();
        public void AddBulkDeals(List<BulkDeal> bulkDeals);
        public List<BulkDeal> GetAllBulkDeals();
        public List<BulkDeal> GetBulkDealsByCompany(string company);
    }
}
