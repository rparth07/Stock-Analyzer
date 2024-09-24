using Stock_Analyzer_Domain.Models;

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
