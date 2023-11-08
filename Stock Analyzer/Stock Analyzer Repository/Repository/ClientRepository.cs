using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Repository.Repository
{
  public class ClientRepository : IClientRepository, IDisposable
  {
    private readonly StockAnalyzerContext _context;
    private readonly IMapper _mapper;

    public ClientRepository(StockAnalyzerContext context, IMapper mapper)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public void AddClients(List<Client> clientsToInsert)
    {
      var clientInfos = _mapper.Map<List<ClientDataModel>>(clientsToInsert);

      _context.Client.AddRange(clientInfos);
      _context.SaveChanges();
    }

    public List<Client> GetAllClients()
    {
      var clients = _context.Client
          .Include("Deals")
          .Include("Deals.Company")
          .AsNoTracking()
          .ToList();

      return _mapper.Map<List<Client>>(clients);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose resources when needed
      }
    }

  }
}
