using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface ICompanyRepository
  {
    public void AddCompanies(List<Company> companiesToInsert);
    public List<Company> GetAllCompaniesWithAllInfo();
    public Company GetCompanyByName(string companyName);
    public List<Company> GetAllCompanies();
  }
}
