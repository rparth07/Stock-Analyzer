using System.ComponentModel.DataAnnotations;

namespace Stock_Analyzer.Dto.Filter
{
  public class FilterDto
  {
    public FilterDto(string filterName)
    {
      FilterName = filterName;
      Criterias = new List<FilterCriteriaDto>();
    }

    [Required]
    public string FilterName { get; set; }

    public List<FilterCriteriaDto> Criterias { get; set; } = new List<FilterCriteriaDto>();

  }
}
