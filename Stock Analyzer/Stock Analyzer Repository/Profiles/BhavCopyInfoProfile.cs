using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Profiles
{
  public class BhavCopyInfoProfile : Profile
  {
    public BhavCopyInfoProfile()
    {
      CreateMap<BhavCopyInfoDataModel, BhavCopyInfo>().MaxDepth(8)
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date));

      CreateMap<BhavCopyInfo, BhavCopyInfoDataModel>().MaxDepth(8)
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date));

    }
  }
}
