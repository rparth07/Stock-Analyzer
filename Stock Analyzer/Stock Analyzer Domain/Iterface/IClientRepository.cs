using Stock_Analyzer_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface IClientRepository
  {
    public void AddClients(List<Client> clientsToInsert);
    public List<Client> GetAllClients();
  }
}
