using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Repository.DataModels
{
  public class NotebookDataModel
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime ContentDate { get; set; }
  }
}
