using System.ComponentModel.DataAnnotations;

namespace Stock_Analyzer.Dto.Filter
{
  public class FilterDto
  {
    public FilterDto(string filterName, string series)
    {
      FilterName = filterName;
      Series = series;
      Criterias = new List<FilterCriteriaDto>();
    }

    public Guid Id { get; set; }

    public string FilterName { get; set; }

    public string Series { get; set; }

    public List<FilterCriteriaDto> Criterias { get; set; } = new List<FilterCriteriaDto>();

  }
}
