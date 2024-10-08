using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Analyzer_Repository.DataModels.Filter
{
  public class FilterResultDataModel
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("FilterCriteriaId")]
    [InverseProperty("FilterResults")]
    public virtual FilterCriteriaDataModel FilterCriteria { get; set; }

    [Required]
    public DateTime CalculationDate { get; set; }

    [Required]
    [ForeignKey("CompanyId")]
    [InverseProperty("FilterResults")]
    public virtual CompanyDataModel Company { get; set; }

    [Required]
    public double Value { get; set; }
  }
}
