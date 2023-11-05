using AutoMapper;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Repository.DataModels.Filter;

namespace Stock_Analyzer_Repository.Profiles
{
  public class FilterProfile : Profile
  {
    public FilterProfile()
    {
      CreateMap<Filter, FilterDataModel>();
      CreateMap<FilterCriteria, FilterCriteriaDataModel>();
      CreateMap<FilterResult, FilterResultDataModel>()
        .ForMember(dest => dest.CalculationDate, opt => opt.MapFrom(src => ToDate(src.CalculationDate)));

      CreateMap<FilterDataModel, Filter>();
      CreateMap<FilterCriteriaDataModel, FilterCriteria>();
      CreateMap<FilterResultDataModel, FilterResult>()
        .ForMember(dest => dest.CalculationDate, opt => opt.MapFrom(src => ToDate(src.CalculationDate)));
    }

    private DateTime ToDate(DateTime calculationDate)
    {
      return calculationDate.Date;
    }
  }
}
