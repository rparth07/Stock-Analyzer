using Stock_Analyzer_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface ICompanyRepository
  {
    public void AddCompanies(List<Company> companiesToInsert);
    public List<Company> GetAllCompaniesWithAllInfo();
    public Company GetCompanyByName(string companyName);
    public List<Company> GetAllCompanies();
    public List<Company> GetCompaniesToInsert(List<Company> companies);
  }
}
