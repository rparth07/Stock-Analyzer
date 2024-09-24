using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stock_Analyzer.Dto.Filter;
using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Service.Interface;

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
    public IActionResult AddFilter(FilterDto filterDto)
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

    [HttpGet("get-filter-result")]
    public IActionResult GetFilterResults(string filterName, DateTime filterDate)
    {
      if (filterName.IsNullOrEmpty() || filterDate == null)
      {
        return Ok();
      }
      filterDate = filterDate.ToLocalTime().Date;
      var filterToExecute = _filterService.GetFilterByName(filterName);
      var filterResult = _filterService.GetFilterResults(filterToExecute, filterDate);

      var result = _mapper.Map<List<FilterResultDto>>(filterResult);
      return Ok(result);
    }

    [HttpGet("execute-all-filters")]
    public IActionResult ExecuteAllFilters(DateTime filterDate)
    {
      filterDate = filterDate.ToLocalTime().Date;
      _filterService.StoreFilterResultsByFilterFor(filterDate);
      return Ok();
    }

    [HttpDelete("delete-filter/{filterName}")]
    public IActionResult DeleteFilter(string filterName)
    {
      try
      {
        _filterService.DeleteFilterByName(filterName);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      return Ok();
    }
  }
}
