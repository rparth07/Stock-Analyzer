using CsvHelper.Configuration.Attributes;

namespace Stock_Analyzer.CSVParserModel
{
  public class ParsedStockInfo
  {
    [Index(0)]
    public string? CompanySymbol { get; set; }

    [Index(1)]
    public string? Series { get; set; }

    [Index(2)]
    public DateTime Date { get; set; }

    [Index(3)]
    public string? PreviousClose { get; set; }

    [Index(4)]
    public string? OpenPrice { get; set; }

    [Index(5)]
    public string? HighPrice { get; set; }

    [Index(6)]
    public string? LowPrice { get; set; }

    [Index(7)]
    public string? LastPrice { get; set; }

    [Index(8)]
    public string? ClosePrice { get; set; }

    [Index(9)]
    public string? AvgPrice { get; set; }

    [Index(10)]
    public string? TtlTrdQnty { get; set; }

    [Index(11)]
    public string? TurnOverLacs { get; set; }

    [Index(12)]
    public string? NoOfTrades { get; set; }

    [Index(13)]
    public string? DeliveryQty { get; set; }

    [Index(14)]
    public string? DeliveryPercentage { get; set; }
  }
}
