using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Analyzer_Repository.DataModels.Filter
{
  public class FilterCriteriaDataModel
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("filterId")]
    [InverseProperty("Criterias")]
    public virtual FilterDataModel FilterDataModel { get; set; }

    [Required]
    public int Sequence { get; set; }

    [Required]
    public string FieldName { get; set; }

    [Required]
    public ChangeType ChangeType { get; set; }

    [Required]
    public LogicalOperator LogicalOperator { get; set; }

    [Required]
    public PeriodType PeriodType { get; set; }

    [Required]
    public int PeriodValue { get; set; }

    public List<FilterResultDataModel> FilterResults { get; set; } = new List<FilterResultDataModel>();

  }
}
