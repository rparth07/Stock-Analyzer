using AutoMapper;
using Stock_Analyzer.Dto;
using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.Profiles
{
  public class NotebookProfile : Profile
  {
    public NotebookProfile()
    {
      CreateMap<NotebookDto, Notebook>()
        .ForMember(dest => dest.ContentDate, opt => opt.MapFrom(src => src.ContentDate.Date));

      CreateMap<Notebook, NotebookDto>()
        .ForMember(dest => dest.ContentDate, opt => opt.MapFrom(src => src.ContentDate.Date));
    }
  }
}
