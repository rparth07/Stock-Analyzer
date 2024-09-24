using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface IClientRepository
  {
    public void AddClients(List<Client> clientsToInsert);
    public List<Client> GetAllClients();
  }
}
