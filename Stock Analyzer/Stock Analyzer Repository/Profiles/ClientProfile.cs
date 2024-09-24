using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Profiles
{
  public class ClientProfile : Profile
  {
    public ClientProfile()
    {
      CreateMap<Client, ClientDataModel>();
      CreateMap<ClientDataModel, Client>();
    }
  }
}
