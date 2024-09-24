using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface IBhavInfoRepository
  {
    public void AddBhavInfos(List<BhavCopyInfo> bhavCopyInfosToInsert);
    public List<BhavCopyInfo> GetAllBhavInfosWithCompany(DateTime date);
    public List<BhavCopyInfo> GetBhavInfosByCompany(string company);
    public List<BhavCopyInfo> GetBhavInfosBetween(DateTime startDate, DateTime endDate);
    public List<BhavCopyInfo> GetAllBhavInfosWithCompanies();
    public List<BhavCopyInfo> GetAllBhavInfos(DateTime filterDate);
    public List<BhavCopyInfo> GetBhvaInfosBy(DateTime fromDate, DateTime toDate, string series);
    public DateTime GetLatestBhavInfoDate();
  }
}
