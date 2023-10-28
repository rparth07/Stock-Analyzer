using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stock_Analyzer.Dto.Filter;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Service;
using Stock_Analyzer_Service.Interface;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stock_Analyzer.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class FilterController : ControllerBase
  {
    private readonly IFilterService _filterService;
    private readonly IMapper _mapper;
    private readonly ILogger<FilterController> _logger;
    public FilterController(IFilterService filterService, IMapper mapper, ILogger<FilterController> logger)
    {
      _filterService = filterService ?? throw new ArgumentNullException(nameof(filterService));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("add-filter")]
    [Consumes("application/json")]
    public IActionResult AddFilterAsync(FilterDto filterDto)
    {
      var filter = _mapper.Map<Filter>(filterDto);

      _filterService.AddFilter(filter);
      return Ok(filterDto);
    }

    [HttpGet("get-filters")]
    public IActionResult GetFilters()
    {
      var filters = _filterService.GetFilters();

      return Ok(_mapper.Map<List<FilterDto>>(filters));
    }

    [HttpGet("execute-filter")]
    public IActionResult ExecuteFilter([FromQuery]string filterName, DateTime filterDate)
    {
      var filterToExecute = _filterService.GetFilterByName(filterName);
      var filterResult = _filterService.ExecuteFilter(filterToExecute, filterDate);

      return Ok(filterResult);
    }
  }
}
