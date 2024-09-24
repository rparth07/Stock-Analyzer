using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer_Domain.Iterface
{
  public interface INotebookRepository
  {
    public Notebook GetNotebook(DateTime notebookDate);
    public List<Notebook> GetNotebooks();
    public void CreateNotebook(Notebook newNotebook);
    public void UpsertNotebook(Notebook notebookToUpdate);
    public void DeleteNotebook(Notebook notebookToDelete);
  }
}
