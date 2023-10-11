using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<BulkDeal> Deals { get; set; } = new List<BulkDeal>();

        public Client()
        {}

        public Client(string name)
        {
            this.Name = name;
        }
    }
}
