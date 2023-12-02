using AutoMapper;
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
  public class NotebookRepository : INotebookRepository
  {
    private readonly StockAnalyzerContext _context;
    private readonly IMapper _mapper;

    public NotebookRepository(StockAnalyzerContext context, IMapper mapper)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public void CreateNotebook(Notebook newNotebook)
    {
      var notebookToAdd = _mapper.Map<NotebookDataModel>(newNotebook);
      _context.Notebook.Add(notebookToAdd);
      _context.SaveChanges();
    }

    public Notebook GetNotebook(DateTime notebookDate)
    {
      var notebook = _context.Notebook
        .FirstOrDefault(_ => _.ContentDate == notebookDate);

      return _mapper.Map<Notebook>(notebook);
    }

    public List<Notebook> GetNotebooks()
    {
      var notebooks = _context.Notebook.
        Where(_ => _.ContentDate >= DateTime.Today.AddMonths(-6));

      return _mapper.Map<List<Notebook>>(notebooks);
    }

    public void UpsertNotebook(Notebook notebook)
    {
      var notebookToUpdate = _context.Notebook
        .FirstOrDefault(_ => _.ContentDate == notebook.ContentDate);

      if(notebookToUpdate != null)
      {
        notebookToUpdate.Content = notebook.Content;
        _context.Notebook.Update(notebookToUpdate);
        _context.SaveChanges();
      } else
      {
        CreateNotebook(notebook);
      }
    }

    public void DeleteNotebook(Notebook notebook)
    {
      var notebookToDelete = _context.Notebook
        .FirstOrDefault(_ => _.ContentDate == notebook.ContentDate);

      if(notebookToDelete != null)
      {
        _context.Notebook.Remove(notebookToDelete);
        _context.SaveChanges();
      }
    }
  }
}
