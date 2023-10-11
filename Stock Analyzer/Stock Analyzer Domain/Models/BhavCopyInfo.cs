using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Analyzer_Domain.Models
{
    public class BhavCopyInfo
    {
        public Guid Id { get; set; }
        
        public virtual Company Company { get; set; }

        public string Series { get; set; }

        public DateTime Date { get; set; }

        public double PreviousClose { get; set; }

        public double OpenPrice { get; set; }

        public double HighPrice { get; set; }

        public double LowPrice { get; set; }

        public double LastPrice { get; set; }

        public double ClosePrice { get; set; }

        public double AvgPrice { get; set; }

        public double TtlTrdQnty { get; set; }

        public double TurnOverLacs { get; set; }

        public double NoOfTrades { get; set; }

        public double DeliveryQty { get; set; }

        public double DeliveryPercentage { get; set; }
    }
}
