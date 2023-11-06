using Stock_Analyzer_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface IBhavInfoRepository
  {
    public void AddBhavInfos(List<BhavCopyInfo> bhavCopyInfosToInsert);
    public List<BhavCopyInfo> GetAllBhavInfosWithCompany(DateTime date);
    public List<BhavCopyInfo> GetBhavInfosByCompany(string company);
    public List<BhavCopyInfo> GetBhavInfosToInsert(List<BhavCopyInfo> bhavInfos);
    public List<BhavCopyInfo> GetAllBhavInfosWithCompanies();
    public List<BhavCopyInfo> GetAllBhavInfos(DateTime filterDate);
  }
}
