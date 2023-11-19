
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Service.Interface;

namespace Stock_Analyzer_Service
{
  public class NotebookService : INotebookService
  {
    private readonly INotebookRepository _notebookRepository;
    public NotebookService(INotebookRepository notebookService)
    {
      _notebookRepository = notebookService ?? throw new ArgumentNullException(nameof(notebookService));
    }

    public void CreateNotebook(DateTime notebookDate)
    {
      var notebook = new Notebook(notebookDate);
      _notebookRepository.CreateNotebook(notebook);
    }

    public Notebook GetNotebook(DateTime notebookDate)
    {
      var notebook = _notebookRepository.GetNotebook(notebookDate);

      if(notebook == null)
      {
        CreateNotebook(notebookDate);
        notebook = _notebookRepository.GetNotebook(notebookDate);
      }

      return notebook;
    }

    public void UpdateNotebook(Notebook notebook)
    {
      if (notebook == null)
        return;

      _notebookRepository.UpdateNotebook(notebook);
    }
  }
}
