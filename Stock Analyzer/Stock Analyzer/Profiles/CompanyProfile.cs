using AutoMapper;
using Stock_Analyzer.Dto;
using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.Profiles
{
  public class CompanyProfile : Profile
  {
    public CompanyProfile()
    {
      CreateMap<Company, CompanyDto>();
    }
  }
}
