using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Profiles
{
  public class BulkDealProfile : Profile
  {
    public BulkDealProfile()
    {
      CreateMap<BulkDeal, BulkDealDataModel>()
        .ForMember(dest => dest.DealDate, opt => opt.MapFrom(src => src.DealDate.Date));

      CreateMap<BulkDealDataModel, BulkDeal>()
        .ForMember(dest => dest.DealDate, opt => opt.MapFrom(src => src.DealDate.Date));
    }
  }
}
