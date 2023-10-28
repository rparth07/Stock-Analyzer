using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Models.Filter
{
  public class FilterResult
  {
    public int Id { get; set; }

    public virtual FilterCriteria FilterCriteria { get; set; }

    public DateTime CalculationDate { get; set; }

    public virtual Company Company { get; set; }

    public double Value { get; set; }
  }
}
