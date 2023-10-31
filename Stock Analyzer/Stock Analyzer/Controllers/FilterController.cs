using AutoMapper;
using FluentDateTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
    public IActionResult ExecuteFilter(string filterName, DateTime filterDate)
    {
      if(filterName.IsNullOrEmpty() || filterDate == null)
      {
        return Ok();
      }
      filterDate = filterDate.ToLocalTime();
      var filterToExecute = _filterService.GetFilterByName(filterName);
      var filterResult = _filterService.ExecuteFilter(filterToExecute, filterDate);

      var result = _mapper.Map<List<FilterResultDto>>(filterResult);
      return Ok(result);
    }

    [HttpGet("execute-all-filters")]
    public IActionResult ExecuteAllFilters()
    {
      var filtersToExecute = _filterService.GetFilters();
      // Here, need to handle scenario where compute avg price and filter criteria execution
      // should consider only the 5 days a week, like if for criteria is executed on monday,
      //then the previous day should be friday, similiar for calculation of avg price.

      /*var fromDate = DateTime.Parse("2023-10-15T07:22:16.0000000Z");
      var toDate = DateTime.Parse("2023-10-28T07:22:16.0000000Z");

      while(fromDate < toDate)
      {
        var filterResult = _filterService.ExecuteFilter(null, fromDate);
        fromDate = fromDate.AddBusinessDays(1);
      }
*/
      return Ok();
    }
  }
}
