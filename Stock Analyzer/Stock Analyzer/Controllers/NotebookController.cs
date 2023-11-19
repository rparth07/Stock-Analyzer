using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stock_Analyzer.Dto;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Service.Interface;

namespace Stock_Analyzer.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class NotebookController : ControllerBase
  {
    private readonly INotebookService _notebookService;
    private readonly IMapper _mapper;
    public NotebookController(INotebookService notebookService, IMapper mapper)
    {
      _notebookService = notebookService ?? throw new ArgumentNullException(nameof(notebookService));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpPost("update-notebook")]
    public IActionResult UpdateNotebook(NotebookDto notebookDto)
    {
      var notebook = _mapper.Map<Notebook>(notebookDto);

      _notebookService.UpdateNotebook(notebook);
      return Ok(notebookDto);
    }

    [HttpGet("get-notebook")]
    public IActionResult GetNotebook([FromQuery]DateTime notebookDate)
    {
      var notebook = _notebookService.GetNotebook(notebookDate);

      return Ok(_mapper.Map<NotebookDto>(notebook));
    }
  }
}
