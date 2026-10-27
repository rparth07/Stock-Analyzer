using AutoMapper;
using Stock_Analyzer.Dto.Filter;
using Stock_Analyzer_Domain.Models.Filter;

namespace Stock_Analyzer.Profiles
{
  public class FilterProfile : Profile
  {
    public FilterProfile()
    {
      CreateMap<FilterDto, Filter>().MaxDepth(8)
        .ForMember(dest => dest.FilterType, opt => opt.MapFrom(src => ConvertToEnum<FilterType>(src.FilterType)));
      CreateMap<FilterCriteriaDto, FilterCriteria>().MaxDepth(8)
        .ForMember(dest => dest.ChangeType, opt => opt.MapFrom(src => ConvertToEnum<ChangeType>(src.ChangeType)))
        .ForMember(dest => dest.LogicalOperator, opt => opt.MapFrom(src => ConvertToEnum<LogicalOperator>(src.LogicalOperator)))
        .ForMember(dest => dest.PeriodType, opt => opt.MapFrom(src => ConvertToEnum<PeriodType>(src.PeriodType)));
      CreateMap<FilterResultDto, FilterResult>().MaxDepth(8)
        .ForMember(dest => dest.CalculationDate, opt => opt.MapFrom(src => src.CalculationDate.Date));

      CreateMap<Filter, FilterDto>().MaxDepth(8);
      CreateMap<FilterCriteria, FilterCriteriaDto>().MaxDepth(8);
      CreateMap<FilterResult, FilterResultDto>().MaxDepth(8)
        .ForMember(dest => dest.CalculationDate, opt => opt.MapFrom(src => src.CalculationDate.Date));
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
