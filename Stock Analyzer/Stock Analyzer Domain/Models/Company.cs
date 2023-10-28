using Stock_Analyzer_Domain.Models.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Models
{
    public class Company
    {
        public Guid Id { get; set; }

        public string Symbol { get; set; }

        public ICollection<BhavCopyInfo> BhavCopyInfos { get; set; } = new List<BhavCopyInfo>();

        public ICollection<BulkDeal> BulkDeals { get; set; } = new List<BulkDeal>();

        public List<FilterResult> FilterResults { get; set; } = new List<FilterResult>();

        public Company()
        {}

        public Company(string name)
        {
            this.Symbol = name;
        }
    }
}
