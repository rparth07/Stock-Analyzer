using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.Dto.Filter
{
  public class FilterResultDto
  {
    public int Id { get; set; }

    public FilterCriteriaDto FilterCriteriaDto { get; set; }

    public DateTime CalculationDate { get; set; }

    public CompanyDto CompanyDto { get; set; }

    public double Value { get; set; }
  }
}
