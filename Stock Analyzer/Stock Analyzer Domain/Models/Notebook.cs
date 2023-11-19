using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Models
{
  public class Notebook
  {
    public Notebook(DateTime contentDate)
    {
      ContentDate = contentDate;
      Content = "";
    }

    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime ContentDate { get; set; }
  }
}
