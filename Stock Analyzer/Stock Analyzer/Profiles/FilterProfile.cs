using AutoMapper;
using Stock_Analyzer.Dto.Filter;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Domain.Models.Filter;

namespace Stock_Analyzer.Profiles
{
  public class FilterProfile : Profile
  {
    public FilterProfile()
    {
      CreateMap<FilterDto, Filter>();
      CreateMap<FilterCriteriaDto, FilterCriteria>()
        .ForMember(dest => dest.ChangeType, opt => opt.MapFrom(src => ConvertToEnum<ChangeType>(src.ChangeType)))
        .ForMember(dest => dest.LogicalOperator, opt => opt.MapFrom(src => ConvertToEnum<LogicalOperator>(src.LogicalOperator)))
        .ForMember(dest => dest.PeriodType, opt => opt.MapFrom(src => ConvertToEnum<PeriodType>(src.PeriodType)));
      CreateMap<FilterResultDto, FilterResult>();

      CreateMap<Filter, FilterDto>();
      CreateMap<FilterCriteria, FilterCriteriaDto>();
      CreateMap<FilterResult, FilterResultDto>();
    }

    public TEnum ConvertToEnum<TEnum>(string strEnumValue) where TEnum : struct
    {
      if (Enum.TryParse(strEnumValue, true, out TEnum result))
      {
        if (Enum.IsDefined(typeof(TEnum), result))
        {
          return result;
        }
      }

      // You can handle errors here, e.g., throw an exception or return a default value
      throw new ArgumentException($"Invalid value for enum {typeof(TEnum)}: {strEnumValue}");
    }
  }
}
