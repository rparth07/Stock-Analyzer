using Stock_Analyzer_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Service.Interface
{
  public interface INotebookService
  {
    public Notebook GetNotebook(DateTime notebookDate);
    public List<Notebook> GetNotebooks();
    public void CreateNotebook(DateTime notebookDate);
    public void UpdateNotebook(Notebook notebookToUpdate);
    public void UpdateNotebooks(List<Notebook> notebooks);
  }
}
