using Stock_Analyzer_Domain.Models.Filter;
using System.ComponentModel.DataAnnotations;

namespace Stock_Analyzer.Dto.Filter
{
  public class FilterDto
  {
    public FilterDto(string filterName, string series, string filterType)
    {
      FilterName = filterName;
      Series = series;
      FilterType = filterType;
      Criterias = new List<FilterCriteriaDto>();
    }

    public Guid Id { get; set; }

    public string FilterName { get; set; }

    public string Series { get; set; }

    public string FilterType { get; set; }

    public List<FilterCriteriaDto> Criterias { get; set; } = new List<FilterCriteriaDto>();

  }
}
