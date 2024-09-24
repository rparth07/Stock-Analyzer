namespace Stock_Analyzer.Dto
{
  public class StockInfoDto
  {
    public string? CompanySymbol { get; set; }

    public string? Series { get; set; }

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
