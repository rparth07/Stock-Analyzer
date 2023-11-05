using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Repository.Profiles
{
  public class BhavCopyInfoProfile : Profile
  {
    public BhavCopyInfoProfile()
    {
      CreateMap<BhavCopyInfoDataModel, BhavCopyInfo>()
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => ToDate(src.Date)));

      CreateMap<BhavCopyInfo, BhavCopyInfoDataModel>()
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => ToDate(src.Date)));

    }

    private DateTime ToDate(DateTime date)
    {
      return date.Date;
    }
  }
}
