using Stock_Analyzer_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface IStockInfoRepository
  {
    public void AddCompanies(List<Company> companiesToInsert);
    public List<Company> GetAllCompaniesWithAllInfo();

    public Company GetCompanyByName(string companyName);
    public List<Company> GetAllCompanies();
    public List<Company> GetCompaniesToInsert(List<Company> companies);

    public void AddClients(List<Client> clientsToInsert);
    public List<Client> GetAllClients();

    public void AddBhavInfos(List<BhavCopyInfo> bhavCopyInfosToInsert);
    public List<BhavCopyInfo> GetAllBhavInfosWithCompany(DateTime date);
    public List<BhavCopyInfo> GetBhavInfosByCompany(string company);
    public List<BhavCopyInfo> GetBhavInfosToInsert(List<BhavCopyInfo> bhavInfos);
    public List<BhavCopyInfo> GetAllBhavInfosWithCompanies();

    public void AddBulkDeals(List<BulkDeal> bulkDealsToInsert);
    public List<BulkDeal> GetAllBulkDeals();
    public List<BulkDeal> GetBulkDealsByCompany(string company);
  }
}
