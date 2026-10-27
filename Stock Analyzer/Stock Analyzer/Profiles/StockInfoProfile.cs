using AutoMapper;
using Stock_Analyzer.CSVParserModel;
using Stock_Analyzer.Dto;
using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.Profiles
{
  public class StockInfoProfile : Profile
  {
    public StockInfoProfile()
    {
      CreateMap<StockInfoDto, BhavCopyInfo>().MaxDepth(8)
          .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date))
          .ForMember(dest => dest.Company, opt => opt.MapFrom(src => new Company(src.CompanySymbol)));

      CreateMap<StockInfoDto, Company>().MaxDepth(8)
          .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.CompanySymbol));

      CreateMap<BhavCopyInfo, StockInfoDto>().MaxDepth(8)
          .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date))
          .ForMember(dest => dest.CompanySymbol, opt => opt.MapFrom(src => src.Company.Symbol))
          .ForMember(dest => dest.Series, opt => opt.MapFrom(src => src.Series));

      CreateMap<ParsedStockInfo, StockInfoDto>().MaxDepth(8)
          .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date))
          .ForMember(dest => dest.PreviousClose, opt => opt.MapFrom(src => ParseOptional(src.PreviousClose)))
          .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => ParseOptional(src.OpenPrice)))
          .ForMember(dest => dest.HighPrice, opt => opt.MapFrom(src => ParseOptional(src.HighPrice)))
          .ForMember(dest => dest.LowPrice, opt => opt.MapFrom(src => ParseOptional(src.LowPrice)))
          .ForMember(dest => dest.LastPrice, opt => opt.MapFrom(src => ParseOptional(src.LastPrice)))
          .ForMember(dest => dest.ClosePrice, opt => opt.MapFrom(src => ParseOptional(src.ClosePrice)))
          .ForMember(dest => dest.AvgPrice, opt => opt.MapFrom(src => ParseOptional(src.AvgPrice)))
          .ForMember(dest => dest.TtlTrdQnty, opt => opt.MapFrom(src => ParseOptional(src.TtlTrdQnty)))
          .ForMember(dest => dest.TurnOverLacs, opt => opt.MapFrom(src => ParseOptional(src.TurnOverLacs)))
          .ForMember(dest => dest.NoOfTrades, opt => opt.MapFrom(src => ParseOptional(src.NoOfTrades)))
          .ForMember(dest => dest.DeliveryQty, opt => opt.MapFrom(src => ParseOptional(src.DeliveryQty)))
          .ForMember(dest => dest.DeliveryPercentage, opt => opt.MapFrom(src => ParseOptional(src.DeliveryPercentage)))
          .ForMember(dest => dest.Series, opt => opt.MapFrom(src => src.Series.Trim()))
          .ForMember(dest => dest.CompanySymbol, opt => opt.MapFrom(src => src.CompanySymbol.Trim()));


      CreateMap<ParsedHistoricalBhavInfo, ParsedStockInfo>().MaxDepth(8)
          .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Date));
    }

    private static double? ParseOptional(string? value) =>
      double.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var number) && double.IsFinite(number) ? number : null;

    private static double ConvertToDouble(string? value)
    {
      if (value != null && value.Contains('-'))
      {
        value = value.Replace('-', ' ');
      }

      value = value?.Trim() != String.Empty ? value?.Trim() : null;
      return Convert.ToDouble(value);
    }
  }
}
