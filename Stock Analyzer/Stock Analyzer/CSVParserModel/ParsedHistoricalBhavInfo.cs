using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace Stock_Analyzer.CSVParserModel
{
  public class ParsedHistoricalBhavInfo
  {
    [JsonProperty("CH_SYMBOL")]
    public string? CompanySymbol { get; set; }

    [JsonProperty("CH_SERIES")]
    public string? Series { get; set; }

    [JsonProperty("CH_MARKET_TYPE")]
    public string? MarketType { get; set; }

    [JsonProperty("CH_TRADE_HIGH_PRICE")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? HighPrice { get; set; }

    [JsonProperty("CH_TRADE_LOW_PRICE")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? LowPrice { get; set; }

    [JsonProperty("CH_OPENING_PRICE")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? OpenPrice { get; set; }

    [JsonProperty("CH_CLOSING_PRICE")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? ClosePrice { get; set; }

    [JsonProperty("CH_LAST_TRADED_PRICE")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? LastPrice { get; set; }

    [JsonProperty("CH_PREVIOUS_CLS_PRICE")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? PreviousClose { get; set; }

    [JsonProperty("CH_TOT_TRADED_QTY")]
    [JsonConverter(typeof(IntegerOrStringConverter))]
    public int? TtlTrdQnty { get; set; }

    [JsonProperty("CH_TOT_TRADED_VAL")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? TurnOverLacs { get; set; }

    [JsonProperty("CH_52WEEK_HIGH_PRICE")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? WeekHigh52 { get; set; }

    [JsonProperty("CH_52WEEK_LOW_PRICE")]
    public double? WeekLow52 { get; set; }

    [JsonProperty("CH_TOTAL_TRADES")]
    [JsonConverter(typeof(IntegerOrStringConverter))]
    public int? NoOfTrades { get; set; }

    [JsonProperty("mTIMESTAMP")]
    [JsonConverter(typeof(CustomDateFormatConverter), "dd-MMM-yyyy")]
    public DateTime Date { get; set; }

    [JsonProperty("COP_DELIV_QTY")]
    [JsonConverter(typeof(IntegerOrStringConverter))]
    public int? DeliveryQty { get; set; }

    [JsonProperty("COP_DELIV_PERC")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? DeliveryPercentage { get; set; }

    [JsonProperty("VWAP")]
    [JsonConverter(typeof(DoubleOrStringConverter))]
    public double? AvgPrice { get; set; }//vwap
  }

  public class Metadata
  {
    [JsonProperty("series")]
    public string Series { get; set; }

    [JsonProperty("fromDate")]
    [JsonConverter(typeof(CustomDateFormatConverter), "dd-MM-yyyy")]
    public DateTime FromDate { get; set; }

    [JsonProperty("toDate")]
    [JsonConverter(typeof(CustomDateFormatConverter), "dd-MM-yyyy")]
    public DateTime ToDate { get; set; }

    [JsonProperty("symbols")]
    public List<string> Symbols { get; set; }
  }

  public class WrapperBhavInfo
  {
    [JsonProperty("data")]
    public List<ParsedHistoricalBhavInfo> Data { get; set; }

    [JsonProperty("meta")]
    public Metadata Metadata { get; set; }
  }

  public class CustomDateFormatConverter : IsoDateTimeConverter
  {
    public CustomDateFormatConverter(string format)
    {
      DateTimeFormat = format;
    }
  }

  public class IntegerOrStringConverter : JsonConverter<int?>
  {
    public override int? ReadJson(JsonReader reader, Type objectType, int? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      JToken token = JToken.Load(reader);

      if (token.Type == JTokenType.String)
      {
        if (int.TryParse(token.Value<string>(), out int intValue))
        {
          return intValue;
        }
      }
      else if (token.Type == JTokenType.Integer)
      {
        return token.Value<int>();
      }

      return null; // Return null if it's not an integer or cannot be parsed as an integer.
    }

    public override void WriteJson(JsonWriter writer, int? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }

  public class DoubleOrStringConverter : JsonConverter<double?>
  {
    public override double? ReadJson(JsonReader reader, Type objectType, double? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      JToken token = JToken.Load(reader);

      if (token.Type == JTokenType.String)
      {
        if (double.TryParse(token.Value<string>(), out double doubleValue))
        {
          return doubleValue;
        }
      }
      else if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
      {
        return token.Value<double>();
      }

      return null; // Return null if it's not a double or cannot be parsed as a double.
    }

    public override void WriteJson(JsonWriter writer, double? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
