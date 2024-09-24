namespace Stock_Analyzer_Domain.Models.Filter
{
  public class FilterCriteria
  {
    public Guid Id { get; set; }

    public virtual Filter Filter { get; set; }

    public int Sequence { get; set; }

    public string FieldName { get; set; }

    public ChangeType ChangeType { get; set; }

    public LogicalOperator LogicalOperator { get; set; }

    public PeriodType PeriodType { get; set; }

    public int PeriodValue { get; set; }

    public List<FilterResult> FilterResults { get; set; } = new List<FilterResult>();
  }
}
