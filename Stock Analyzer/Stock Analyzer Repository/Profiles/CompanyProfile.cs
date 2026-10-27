using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Profiles
{
  public class CompanyProfile : Profile
  {
    public CompanyProfile()
    {
      CreateMap<CompanyDataModel, Company>().MaxDepth(8);
      CreateMap<Company, CompanyDataModel>().MaxDepth(8);
    }
  }
}
