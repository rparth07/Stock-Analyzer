using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

//using Newtonsoft.Json;
using Stock_Analyzer.CSVParserModel;
using Stock_Analyzer.Dto;
using Stock_Analyzer.Helper;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Service.Interface;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Stock_Analyzer.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class StockAnalyzerController : ControllerBase
  {
    private readonly IStockInfoService _stockInfoService;
    private readonly IMapper _mapper;
    private readonly ILogger<StockAnalyzerController> _logger;

    private static readonly string baseUrl = "https://www.nseindia.com";
    private static readonly string allReportsUrl = baseUrl + "/all-reports";
    private static readonly string bhavCopyReportUrl = baseUrl + "/api/reports?archives=[{\"name\":\"Full Bhavcopy and Security Deliverable data\",\"type\":\"daily-reports\",\"category\":\"capital-market\",\"section\":\"equities\"}]&type=equities&mode=single";
    private static readonly string bulkDealUrl = baseUrl + "/api/historical/bulk-deals?";
    private static readonly string multipleBhavInfoUrl = baseUrl + "/api/historical/securityArchives?";

    public StockAnalyzerController(IStockInfoService stockInfoService, IMapper mapper, ILogger<StockAnalyzerController> logger)
    {
      _stockInfoService = stockInfoService ?? throw new ArgumentNullException(nameof(stockInfoService));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("health-report")]
    public IActionResult HealthUpdate()
    {
      return Ok(new { message = "I'm up and running!" });
    }

    [HttpPost("analyze-bhav-data-between", Name = "AnalyzeBhavDataFileBetween")]
    public async Task<IActionResult> AnalyzeBhavInfoDataFileBetween([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
      //2018-06-22T08:00:19Z
      startDate = startDate.ToLocalTime();
      endDate = endDate.ToLocalTime();
      List<Company> companies = _stockInfoService.GetAllCompanies();
      try
      {
        foreach (var company in companies)
        {
          List<StockInfoDto> stockInfoDtos = new List<StockInfoDto>();
          List<ParsedStockInfo> parsedStocknfos = await GetParsedBhavInfoCSVDataBetween(startDate, endDate, company.Symbol);
          stockInfoDtos = _mapper.Map<List<StockInfoDto>>(parsedStocknfos);


          List<Company> companiesToAdd = _mapper.Map<List<Company>>(stockInfoDtos);
          _stockInfoService.AddCompanies(companiesToAdd);

          List<BhavCopyInfo> bhavInfos = _mapper.Map<List<BhavCopyInfo>>(stockInfoDtos);
          _stockInfoService.AddBhavInfos(bhavInfos);
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message.ToString());
      }

      return Ok(new { message = "Analysis completed successfully!" });
    }

    [HttpPost("analyze-bhav-data", Name = "AnalyzeBhavDataFileOfDate")]
    public async Task<IActionResult> AnalyzeBhavInfoDataOfDate([FromQuery] DateTime date)
    {
      //2023-11-06T08:00:19Z
      date = date.ToLocalTime();
      List<ParsedStockInfo> parsedStocknfos = await GetParsedBhavInfoCSVDataFor(date);
      List<StockInfoDto> stockInfoDtos = _mapper.Map<List<StockInfoDto>>(parsedStocknfos);

      List<Company> companies = _mapper.Map<List<Company>>(stockInfoDtos);
      _stockInfoService.AddCompanies(companies);

      List<BhavCopyInfo> bhavInfos = _mapper.Map<List<BhavCopyInfo>>(stockInfoDtos);
      _stockInfoService.AddBhavInfos(bhavInfos);
      //Need to handle this ok scenarios
      return Ok(new { message = "Analysis completed successfully!" });
    }

    [HttpPost("analyze-daily-bhav-data")]
    public async Task<IActionResult> AnalyzeBhavDataFileOfToday()
    {
      List<ParsedStockInfo> parsedStocknfos = await GetParsedBhavInfoCSVDataFor(DateTime.Today);
      List<StockInfoDto> stockInfoDtos = _mapper.Map<List<StockInfoDto>>(parsedStocknfos);

      List<Company> companies = _mapper.Map<List<Company>>(stockInfoDtos);
      _stockInfoService.AddCompanies(companies);

      List<BhavCopyInfo> bhavInfos = _mapper.Map<List<BhavCopyInfo>>(stockInfoDtos);
      _stockInfoService.AddBhavInfos(bhavInfos);

      return Ok(new { message = "Analysis completed successfully!" });
    }

    [HttpPost("analyze-bhav-data-by-file"), DisableRequestSizeLimit]
    public IActionResult AnalyzeBhavDataFile()
    {
      IFormFile? file = Request.Form.Files[0];
      List<ParsedStockInfo> parsedStocknfos = new ParsedStockInfo().ParseData(file);
      List<StockInfoDto> stockInfoDtos = _mapper.Map<List<StockInfoDto>>(parsedStocknfos);

      List<Company> companies = _mapper.Map<List<Company>>(stockInfoDtos);
      _stockInfoService.AddCompanies(companies);

      List<BhavCopyInfo> bhavInfos = _mapper.Map<List<BhavCopyInfo>>(stockInfoDtos);
      _stockInfoService.AddBhavInfos(bhavInfos);

      return Ok(new { message = "Analysis completed successfully!" });
    }

    [HttpGet("bhav-infos")]
    public IActionResult GetBhavInfosByCompany([FromQuery] string company)
    {
      List<BhavCopyInfo> bhavInfos = _stockInfoService.GetBhavInfosByCompany(company);
      List<StockInfoDto> bhavInfoDtos = _mapper.Map<List<StockInfoDto>>(bhavInfos);
      return Ok(bhavInfoDtos);
    }

    [HttpGet("bulk-deals")]
    public IActionResult GetBulkDealsByCompany([FromQuery] string company)
    {
      List<BulkDeal> bulkDeals = _stockInfoService.GetBulkDealsByCompany(company);
      List<BulkDealDto> bulkDealDtos = _mapper.Map<List<BulkDealDto>>(bulkDeals);
      return Ok(bulkDealDtos);
    }

    [HttpGet("bhav-infos-between")]
    public IActionResult GetBhavInfosBetween([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
      startDate = startDate.Date; endDate = endDate.Date;
      List<BhavCopyInfo> bhavInfos = _stockInfoService.GetBhavInfosBetween(startDate, endDate);
      List<StockInfoDto> bhavInfoDtos = _mapper.Map<List<StockInfoDto>>(bhavInfos);
      return Ok(bhavInfoDtos);
    }

    [HttpGet("bhav-infos-of-all-companies")]
    public IActionResult GetAllBhavInfosWithCompanies()
    {
      List<BhavCopyInfo> bhavInfos = _stockInfoService.GetAllBhavInfosWithCompanies();
      List<StockInfoDto> bhavInfoDtos = _mapper.Map<List<StockInfoDto>>(bhavInfos);
      return Ok(bhavInfoDtos);
    }

    [HttpGet("company-detail")]
    public IActionResult GetAllCompaniesWithAllInfo([FromQuery] string symbol)
    {
      Company company = _stockInfoService.GetCompanyByName(symbol);
      CompanyDto companyDto = _mapper.Map<CompanyDto>(company);
      return Ok(companyDto);
    }

    [HttpGet("all-info-of-companies")]
    public IActionResult GetAllCompaniesWithAllInfo()
    {
      List<Company> companies = _stockInfoService.GetAllCompaniesWithAllInfo();
      List<CompanyDto> companyDtos = _mapper.Map<List<CompanyDto>>(companies);
      return Ok(companyDtos);
    }

    [HttpGet("companies")]
    public IActionResult GetAllCompanies()
    {
      List<Company> companies = _stockInfoService.GetAllCompanies();
      List<CompanyDto> companyDtos = _mapper.Map<List<CompanyDto>>(companies);
      return Ok(companyDtos);
    }

    [HttpPost("analyze-bulk-deals-between")]
    public async Task<IActionResult> AnalyzeBulkDealsBetweenDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
      startDate = startDate.ToLocalTime();
      endDate = endDate.ToLocalTime();
      List<ParsedBulkDeal> parsedBulkDeals = await GetParsedBulkDealCSVDataBetween(startDate, endDate);
      List<BulkDealDto> bulkDealDtos = _mapper.Map<List<BulkDealDto>>(parsedBulkDeals);

      List<Client> clients = _mapper.Map<List<Client>>(bulkDealDtos);
      _stockInfoService.AddClients(clients);

      List<Company> companies = _mapper.Map<List<Company>>(bulkDealDtos);
      _stockInfoService.AddCompanies(companies);

      List<BulkDeal> bulkDeals = _mapper.Map<List<BulkDeal>>(bulkDealDtos);
      _stockInfoService.AddBulkDeals(bulkDeals);

      return Ok(new { message = "Analysis completed successfully!" });
    }

    [HttpPost("analyze-bulk-deals")]
    public async Task<IActionResult> AnalyzeBulkDealsDataOfDate([FromQuery] DateTime date)
    {
      date = date.ToLocalTime();
      List<ParsedBulkDeal> parsedBulkDeals = await GetParsedBulkDealCSVDataBetween(date, date);
      List<BulkDealDto> bulkDealDtos = _mapper.Map<List<BulkDealDto>>(parsedBulkDeals);

      List<Client> clients = _mapper.Map<List<Client>>(bulkDealDtos);
      _stockInfoService.AddClients(clients);

      List<Company> companies = _mapper.Map<List<Company>>(bulkDealDtos);
      _stockInfoService.AddCompanies(companies);

      List<BulkDeal> bulkDeals = _mapper.Map<List<BulkDeal>>(bulkDealDtos);
      _stockInfoService.AddBulkDeals(bulkDeals);

      return Ok(new { message = "Analysis completed successfully!" });
    }

    [HttpPost("analyze-daily-bulk-deals")]
    public async Task<IActionResult> AnalyzeBulkDealsDataFileOfToday()
    {
      List<ParsedBulkDeal> parsedBulkDeals = await GetParsedBulkDealCSVDataBetween(DateTime.Today, DateTime.Today);
      List<BulkDealDto> bulkDealDtos = _mapper.Map<List<BulkDealDto>>(parsedBulkDeals);

      List<Client> clients = _mapper.Map<List<Client>>(bulkDealDtos);
      _stockInfoService.AddClients(clients);

      List<Company> companies = _mapper.Map<List<Company>>(bulkDealDtos);
      _stockInfoService.AddCompanies(companies);

      List<BulkDeal> bulkDeals = _mapper.Map<List<BulkDeal>>(bulkDealDtos);
      _stockInfoService.AddBulkDeals(bulkDeals);

      return Ok(new { message = "Analysis completed successfully!" });
    }

    [HttpGet("bulk-deals-between")]
    public IActionResult GetBulkDealsBetween(DateTime startDate, DateTime endDate)
    {
      startDate = startDate.Date; endDate = endDate.Date;

      List<BulkDeal> bulkDeals = _stockInfoService.GetBulkDealsBetween(startDate, endDate);
      List<BulkDealDto> bulkDealDtos = _mapper.Map<List<BulkDealDto>>(bulkDeals);
      return Ok(bulkDealDtos);
    }

    [HttpGet("all-bulk-deals")]
    public IActionResult GetAllBulkDeals()
    {
      List<BulkDeal> bulkDeals = _stockInfoService.GetAllBulkDeals();
      List<BulkDealDto> bulkDealsDto = _mapper.Map<List<BulkDealDto>>(bulkDeals);
      return Ok(bulkDealsDto);
    }

    [HttpGet("all-clients")]
    public IActionResult GetAllClients()
    {
      List<Client> clients = _stockInfoService.GetAllClients();
      List<ClientDto> clientDtos = _mapper.Map<List<ClientDto>>(clients);
      return Ok(clientDtos);
    }

    private static string UpdateReportUrlWithDate(DateTime date)
    {
      //e.g. &date=01-Sep-2023
      return bhavCopyReportUrl + "&date=" + date.ToString("dd-MMM-yyyy");
    }

    private static string UpdateBulkDealUrlWithDates(DateTime startDate, DateTime endDate)
    {
      //e.g. &from=01-06-2022&to=01-06-2023&csv=true
      return bulkDealUrl + "from=" + startDate.ToString("dd-MM-yyyy") + "&to=" + endDate.ToString("dd-MM-yyyy") + "&csv=true";
    }

    private static string UpdateMultipleBhavInfoUrlWithDates(DateTime startDate, DateTime endDate, string companySymbol)
    {
      //from=23-03-2023&to=23-09-2023&symbol=ACC&dataType=priceVolumeDeliverable&series=ALL
      return multipleBhavInfoUrl + "from=" + startDate.ToString("dd-MM-yyyy") + "&to=" + endDate.ToString("dd-MM-yyyy") + "&symbol=" + companySymbol + "&dataType=priceVolumeDeliverable&series=ALL";
    }

    private async Task<List<ParsedBulkDeal>> GetParsedBulkDealCSVDataBetween(DateTime startDate, DateTime endDate)
    {
      try
      {
        var updatedBulkURL = UpdateBulkDealUrlWithDates(startDate, endDate);
        var fileResponse = await GetFileResponseAsync(updatedBulkURL);

        if (fileResponse != null)
        {
          using StreamReader reader = new StreamReader(fileResponse, Encoding.UTF8);
          List<ParsedBulkDeal> parsedBulkDeals = new ParsedBulkDeal().ParseData(reader);
          return parsedBulkDeals;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Try again after sometime, An error occurred: {ex.Message}");
      }
      return new List<ParsedBulkDeal>();
    }

    private async Task<List<ParsedStockInfo>> GetParsedBhavInfoCSVDataBetween(DateTime startDate, DateTime endDate, string companySymbol)
    {
      try
      {
        var updatedURL = UpdateMultipleBhavInfoUrlWithDates(startDate, endDate, companySymbol);
        var fileResponse = await GetJsonAsync(updatedURL);

        if (fileResponse != null)
        {
          List<ParsedHistoricalBhavInfo> parsedStockInfos = fileResponse.Data;
          return _mapper.Map<List<ParsedStockInfo>>(parsedStockInfos.Where(_ => _.MarketType == "N").ToList());
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Try again after sometime, An error occurred: {ex.Message}");
      }
      return new List<ParsedStockInfo>();
    }

    private async Task<List<ParsedStockInfo>> GetParsedBhavInfoCSVDataFor(DateTime date)
    {
      try
      {
        var updatedURL = UpdateReportUrlWithDate(date);
        var fileResponse = await GetFileResponseAsync(updatedURL);

        if (fileResponse != null)
        {
          using StreamReader reader = new StreamReader(fileResponse, Encoding.UTF8);
          List<ParsedStockInfo> parsedStockInfos = new ParsedStockInfo().ParseData(reader);
          return parsedStockInfos;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Try again after sometime, An error occurred: {ex.Message}");
      }
      return new List<ParsedStockInfo>();
    }

    private static async Task<Stream> GetFileResponseAsync(string url)
    {
      var handler = new HttpClientHandler
      {
        CookieContainer = new CookieContainer(),
        UseCookies = true,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
      };
      var client = new HttpClient(handler);
      client.DefaultRequestHeaders.Add("Accept", "*/*");
      client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
      client.DefaultRequestHeaders.Add("Connection", "keep-alive");

      var allReportsUrlResponse = await client.GetAsync(allReportsUrl);

      if (allReportsUrlResponse.IsSuccessStatusCode)
      {
        var cookies = handler.CookieContainer.GetCookies(new Uri(allReportsUrl));
        handler.CookieContainer.Add(new Uri(url), cookies);

        var fileResponse = await client.GetStreamAsync(url);

        return fileResponse;
      }
      else
      {
        Console.WriteLine($"When tried to Set Cookie, Request failed with status code: {allReportsUrlResponse.StatusCode}");
        return null;
      }

    }

    //TODO: Need to implement it to get the json data
    private static async Task<WrapperBhavInfo?> GetJsonAsync(string url)
    {
      var handler = new HttpClientHandler
      {
        CookieContainer = new CookieContainer(),
        UseCookies = true,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
      };
      var client = new HttpClient(handler);
      client.DefaultRequestHeaders.Add("Accept", "*/*");
      client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
      client.DefaultRequestHeaders.Add("Connection", "keep-alive");

      var allReportsUrlResponse = await client.GetAsync(allReportsUrl);

      if (allReportsUrlResponse.IsSuccessStatusCode)
      {
        var cookies = handler.CookieContainer.GetCookies(new Uri(allReportsUrl));
        handler.CookieContainer.Add(new Uri(url), cookies);

        var response = await client.GetAsync(url);
        if (response != null)
        {
          var contentEncoding = response.Content.Headers.ContentEncoding.ToString();
          try
          {
            Stream responseStream = await DecodeStream(response, contentEncoding);

            using var reader = new StreamReader(responseStream);
            var responseContent = await reader.ReadToEndAsync();
            Console.WriteLine($"Decompressed Response: {responseContent}");

            var jr = "{\"data\":[{\"_id\":\"67754b93d3205bf7e5c32e1b\",\"CH_SYMBOL\":\"574GS2026\",\"CH_SERIES\":\"GS\",\"CH_MARKET_TYPE\":\"N\",\"CH_TIMESTAMP\":\"2025-01-01\",\"TIMESTAMP\":\"2024-12-31T18:30:00.000Z\",\"CH_TRADE_HIGH_PRICE\":98.9,\"CH_TRADE_LOW_PRICE\":98.9,\"CH_OPENING_PRICE\":98.9,\"CH_CLOSING_PRICE\":98.9,\"CH_LAST_TRADED_PRICE\":98.9,\"CH_PREVIOUS_CLS_PRICE\":99,\"CH_TOT_TRADED_QTY\":20,\"CH_TOT_TRADED_VAL\":1978,\"CH_52WEEK_HIGH_PRICE\":103.6,\"CH_52WEEK_LOW_PRICE\":95.31,\"CH_TOTAL_TRADES\":1,\"CH_ISIN\":\"IN0020210186\",\"createdAt\":\"2025-01-01T14:05:07.300Z\",\"updatedAt\":\"2025-01-01T14:05:07.300Z\",\"__v\":0,\"COP_DELIV_QTY\":20,\"COP_DELIV_PERC\":100,\"VWAP\":98.9,\"mTIMESTAMP\":\"01-Jan-2025\"},{\"_id\":\"67767fff99c47fa1f17101fe\",\"CH_SYMBOL\":\"574GS2026\",\"CH_SERIES\":\"GS\",\"CH_MARKET_TYPE\":\"N\",\"CH_TIMESTAMP\":\"2025-01-02\",\"TIMESTAMP\":\"2025-01-01T18:30:00.000Z\",\"CH_TRADE_HIGH_PRICE\":99.05,\"CH_TRADE_LOW_PRICE\":98.9,\"CH_OPENING_PRICE\":98.9,\"CH_CLOSING_PRICE\":99.05,\"CH_LAST_TRADED_PRICE\":99.05,\"CH_PREVIOUS_CLS_PRICE\":98.9,\"CH_TOT_TRADED_QTY\":95,\"CH_TOT_TRADED_VAL\":9395.65,\"CH_52WEEK_HIGH_PRICE\":103.6,\"CH_52WEEK_LOW_PRICE\":95.31,\"CH_TOTAL_TRADES\":2,\"CH_ISIN\":\"IN0020210186\",\"createdAt\":\"2025-01-02T12:01:03.451Z\",\"updatedAt\":\"2025-01-02T12:01:03.451Z\",\"__v\":0,\"COP_DELIV_QTY\":95,\"COP_DELIV_PERC\":100,\"VWAP\":98.9,\"mTIMESTAMP\":\"02-Jan-2025\"},{\"_id\":\"677bc60097f58fe88f945465\",\"CH_SYMBOL\":\"574GS2026\",\"CH_SERIES\":\"GS\",\"CH_MARKET_TYPE\":\"N\",\"CH_TIMESTAMP\":\"2025-01-06\",\"TIMESTAMP\":\"2025-01-05T18:30:00.000Z\",\"CH_TRADE_HIGH_PRICE\":99,\"CH_TRADE_LOW_PRICE\":97.91,\"CH_OPENING_PRICE\":97.93,\"CH_CLOSING_PRICE\":97.91,\"CH_LAST_TRADED_PRICE\":97.91,\"CH_PREVIOUS_CLS_PRICE\":99.05,\"CH_TOT_TRADED_QTY\":200,\"CH_TOT_TRADED_VAL\":19589.45,\"CH_52WEEK_HIGH_PRICE\":103.6,\"CH_52WEEK_LOW_PRICE\":95.31,\"CH_TOTAL_TRADES\":4,\"CH_ISIN\":\"IN0020210186\",\"createdAt\":\"2025-01-06T12:01:04.448Z\",\"updatedAt\":\"2025-01-06T12:01:04.448Z\",\"__v\":0,\"COP_DELIV_QTY\":100,\"COP_DELIV_PERC\":50,\"VWAP\":97.95,\"mTIMESTAMP\":\"06-Jan-2025\"},{\"_id\":\"677e690014c969f7186432ca\",\"CH_SYMBOL\":\"574GS2026\",\"CH_SERIES\":\"GS\",\"CH_MARKET_TYPE\":\"N\",\"CH_TIMESTAMP\":\"2025-01-08\",\"TIMESTAMP\":\"2025-01-07T18:30:00.000Z\",\"CH_TRADE_HIGH_PRICE\":98.25,\"CH_TRADE_LOW_PRICE\":98.25,\"CH_OPENING_PRICE\":98.25,\"CH_CLOSING_PRICE\":98.25,\"CH_LAST_TRADED_PRICE\":98.25,\"CH_PREVIOUS_CLS_PRICE\":97.91,\"CH_TOT_TRADED_QTY\":2,\"CH_TOT_TRADED_VAL\":196.5,\"CH_52WEEK_HIGH_PRICE\":103.6,\"CH_52WEEK_LOW_PRICE\":95.31,\"CH_TOTAL_TRADES\":1,\"CH_ISIN\":\"IN0020210186\",\"createdAt\":\"2025-01-08T12:01:04.325Z\",\"updatedAt\":\"2025-01-08T12:01:04.325Z\",\"__v\":0,\"COP_DELIV_QTY\":2,\"COP_DELIV_PERC\":100,\"VWAP\":98.25,\"mTIMESTAMP\":\"08-Jan-2025\"}],\"meta\":{\"series\":\"ALL\",\"fromDate\":\"01-01-2025\",\"toDate\":\"08-01-2025\",\"symbols\":[\"574GS2026\",\"574GS2026\"]}}";

            // Deserialize the JSON response to the WrapperBhavInfo object
            var result = JsonConvert.DeserializeObject<WrapperBhavInfo>(jr);

            if (result != null)
            {
              Console.WriteLine("Deserialization Successful!");
              Console.WriteLine($"Number of records: {result.Data.Count}");
            }
            else
            {
              Console.WriteLine("Deserialization failed!");
            }
            var jsonResponse = JsonConvert.DeserializeObject<WrapperBhavInfo>(responseContent);
            var apiResponse = JsonConvert.DeserializeObject<WrapperBhavInfo>(responseContent);
            return apiResponse;
          }
          catch (Exception ex)
          {
            throw new Exception(response.ToString(), ex);
          }
        }
        else
        {
          string msg = $"Not able to process {url}";
          Console.WriteLine(msg);
          throw new Exception(msg);
        }
      }
      else
      {
        Console.WriteLine($"When tried to Set Cookie, Request failed with status code: {allReportsUrlResponse.StatusCode}");
        return null;
      }
    }

    private static async Task<Stream> DecodeStream(HttpResponseMessage response, string? contentEncoding)
    {
      var responseStream = await response.Content.ReadAsStreamAsync();
      if (contentEncoding.Contains("gzip"))
      {
        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
      }
      else if (contentEncoding.Contains("deflate"))
      {
        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
      }
      else if (contentEncoding.Contains("br"))
      {
        responseStream = new BrotliStream(responseStream, CompressionMode.Decompress);
      }

      return responseStream;
    }
  }
}
