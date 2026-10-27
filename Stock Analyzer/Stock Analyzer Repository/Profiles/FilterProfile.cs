using AutoMapper;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Repository.DataModels.Filter;

namespace Stock_Analyzer_Repository.Profiles
{
  public class FilterProfile : Profile
  {
    public FilterProfile()
    {
      CreateMap<Filter, FilterDataModel>().MaxDepth(8);
      CreateMap<FilterCriteria, FilterCriteriaDataModel>().MaxDepth(8);
      CreateMap<FilterResult, FilterResultDataModel>().MaxDepth(8)
        .ForMember(dest => dest.CalculationDate, opt => opt.MapFrom(src => src.CalculationDate.Date));

      CreateMap<FilterDataModel, Filter>().MaxDepth(8);
      CreateMap<FilterCriteriaDataModel, FilterCriteria>().MaxDepth(8);
      CreateMap<FilterResultDataModel, FilterResult>().MaxDepth(8)
        .ForMember(dest => dest.CalculationDate, opt => opt.MapFrom(src => src.CalculationDate.Date));
    }
  }
}
