using Stock_Analyzer_Domain.Models.Filter;

namespace Stock_Analyzer.Dto.Filter
{
  public class FilterCriteriaDto
  {
    public int Sequence { get; set; }

    public string FieldName { get; set; }

    public string ChangeType { get; set; }

    public string LogicalOperator { get; set; }

    public string PeriodType { get; set; }

    public int PeriodValue { get; set; }

    public List<FilterResultDto> FilterResults { get; set; } = new List<FilterResultDto>();

  }
}
