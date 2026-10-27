using AutoMapper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository.Profiles
{
  public class ClientProfile : Profile
  {
    public ClientProfile()
    {
      CreateMap<Client, ClientDataModel>().MaxDepth(8);
      CreateMap<ClientDataModel, Client>().MaxDepth(8);
    }
  }
}
