using Stock_Analyzer_Domain.Models.Filter;
using Stock_Analyzer_Domain.Models;

namespace Stock_Analyzer.Dto.Filter
{
  public class FilterResultDto
  {
    public int Id { get; set; }

    public FilterCriteriaDto FilterCriteria { get; set; }

    public DateTime CalculationDate { get; set; }

    public CompanyDto Company { get; set; }

    public double Value { get; set; }
  }
}
