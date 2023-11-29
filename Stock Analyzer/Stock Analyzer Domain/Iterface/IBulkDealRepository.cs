using Stock_Analyzer_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface IBulkDealRepository
  {
    public void AddBulkDeals(List<BulkDeal> bulkDealsToInsert);
    public List<BulkDeal> GetAllBulkDeals(DateTime filterDate);
    public List<BulkDeal> GetBulkDealsBetween(DateTime startDate, DateTime endDate);
    public List<BulkDeal> GetAllBulkDeals();
    public List<BulkDeal> GetBulkDealsByCompany(string company);
  }
}
