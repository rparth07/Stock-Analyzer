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
  public class ClientProfile : Profile
  {
    public ClientProfile()
    {
      CreateMap<Client, ClientDataModel>();
      CreateMap<ClientDataModel, Client>();
    }
  }
}
