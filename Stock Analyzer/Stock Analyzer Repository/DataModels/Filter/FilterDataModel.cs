using Stock_Analyzer_Domain.Models.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Repository.DataModels.Filter
{
  public class FilterDataModel
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string FilterName { get; set; }

    [Required]
    public string Series { get; set; }

    [Required]
    public FilterType FilterType { get; set; }

    public List<FilterCriteriaDataModel> Criterias { get; set; } = new List<FilterCriteriaDataModel>();
  }
}
