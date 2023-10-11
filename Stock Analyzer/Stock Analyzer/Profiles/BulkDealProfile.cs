using AutoMapper;
using CsvHelper;
using Stock_Analyzer.CSVParserModel;
using Stock_Analyzer.Dto;
using Stock_Analyzer.Helper;
using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.Profiles
{
    public class BulkDealProfile : Profile
    {
        public BulkDealProfile()
        {
            CreateMap<ParsedBulkDeal, BulkDealDto>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => ConvertToLong(src.Quantity)))
                .ForMember(dest => dest.TradePrice, opt => opt.MapFrom(src => ConvertToDouble(src.TradePrice)))
                .ForMember(dest => dest.StockAction, opt => opt.MapFrom(src => ConvertToEnum(src.StockAction)));

            CreateMap<BulkDeal, BulkDealDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
                .ForMember(dest => dest.CompanySymbol, opt => opt.MapFrom(src => src.Company.Symbol));

            CreateMap<BulkDealDto, Client>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ClientName));

            CreateMap<BulkDealDto, Company>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.CompanySymbol));

            CreateMap<BulkDealDto, BulkDeal>()
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => new Client(src.ClientName)))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => new Company(src.CompanySymbol)));

            CreateMap<Client, ClientDto>();
        }

        public static StockAction ConvertToEnum(string strEnumValue)
        {
            return (StockAction)Enum.Parse(typeof(StockAction), strEnumValue, true);
        }

        private static long ConvertToLong(string? value)
        {
            return Convert.ToInt64(ConvertToDouble(value));
        }

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
