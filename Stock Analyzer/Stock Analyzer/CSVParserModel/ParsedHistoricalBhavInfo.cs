using CsvHelper.Configuration.Attributes;

namespace Stock_Analyzer.CSVParserModel
{
    public class ParsedHistoricalBhavInfo
    {
        [Index(1)]
        public string? CompanySymbol { get; set; }

        [Index(2)]
        public string? Series { get; set; }

        [Index(3)]
        public string? MarketType { get; set; }

        [Index(4)]
        public string? HighPrice { get; set; }

        [Index(5)]
        public string? LowPrice { get; set; }

        [Index(6)]
        public string? OpenPrice { get; set; }

        [Index(7)]
        public string? ClosePrice { get; set; }

        [Index(8)]
        public string? LastPrice { get; set; }

        [Index(9)]
        public string? PreviousClose { get; set; }

        [Index(10)]
        public string? TtlTrdQnty { get; set; }

        [Index(11)]
        public string? TurnOverLacs { get; set; }

        [Index(12)]
        public string? WeekHigh52 { get; set; }

        [Index(13)]
        public string? WeekLow52 { get; set; }

        [Index(14)]
        public string? NoOfTrades { get; set; }

        [Index(16)]
        public DateTime Date { get; set; }

        [Index(21)]
        public string? DeliveryQty { get; set; }

        [Index(22)]
        public string? DeliveryPercentage { get; set; }
        
        [Index(23)]
        public string? AvgPrice { get; set; }//vwap
    }

    public class Metadata
    {
        [Index(0)]
        public string Series { get; set; }

        [Index(1)]
        public DateTime FromDate { get; set; }

        [Index(2)]
        public DateTime ToDate { get; set; }

        [Index(3)]
        public List<string> Symbols { get; set; }
    }

    public class WrapperBhavInfo
    {
        public List<ParsedHistoricalBhavInfo> Data { get; set; }

        public Metadata Metadata { get; set; }
    }
}
