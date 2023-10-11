using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Repository.DataModels
{
    public class CompanyDataModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Symbol { get; set; }

        public ICollection<BhavCopyInfoDataModel> BhavCopyInfos { get; set; } = new List<BhavCopyInfoDataModel>();

        public ICollection<BulkDealDataModel> BulkDeals { get; set; } = new List<BulkDealDataModel>();

    }
}
