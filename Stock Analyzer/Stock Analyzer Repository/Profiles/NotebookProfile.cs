using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Profiles
{
  public class NotebookProfile : Profile
  {
    public NotebookProfile()
    {
      CreateMap<Notebook, NotebookDataModel>()
        .ForMember(dest => dest.ContentDate, opt => opt.MapFrom(src => src.ContentDate.Date));

      CreateMap<NotebookDataModel, Notebook>()
        .ForMember(dest => dest.ContentDate, opt => opt.MapFrom(src => src.ContentDate.Date));
    }
  }
}
