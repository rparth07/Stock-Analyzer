using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Profiles
{
  public class BhavCopyInfoProfile : Profile
  {
    public BhavCopyInfoProfile()
    {
      CreateMap<BhavCopyInfoDataModel, BhavCopyInfo>()
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date));

      CreateMap<BhavCopyInfo, BhavCopyInfoDataModel>()
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date));

    }
  }
}
