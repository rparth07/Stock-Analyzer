using Stock_Analyzer_Domain.Models;

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
