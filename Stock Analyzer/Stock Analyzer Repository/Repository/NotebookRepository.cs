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

    public void UpdateNotebook(Notebook notebook)
    {
      var notebookToUpdate = _context.Notebook
        .First(_ => _.ContentDate == notebook.ContentDate);

      notebookToUpdate.Content = notebook.Content;
      _context.Notebook.Update(notebookToUpdate);
      _context.SaveChanges();
    }
  }
}
