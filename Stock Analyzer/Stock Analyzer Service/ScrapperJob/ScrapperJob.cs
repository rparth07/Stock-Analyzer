namespace Stock_Analyzer_Service.ScrapperJob
{
  public class ScrapperJob
  {
    private HttpClient _client = new HttpClient();

    public async Task ExecuteAsync()
    {
      // Your job logic goes here
      Console.WriteLine("Scheduling job is running!");

      await AnalyzeBhavInfoAsync();
      await AnalyzeBulkDealAsync();
      Console.WriteLine("Scheduling job completed successfully!");
    }

    private async Task AnalyzeBhavInfoAsync()
    {
      var response = await _client
        .PostAsync("https://localhost:7012/StockAnalyzer/analyze-daily-bhav-data", null);
      response.EnsureSuccessStatusCode();
    }

    private async Task AnalyzeBulkDealAsync()
    {
      var response = await _client
        .PostAsync("https://localhost:7012/StockAnalyzer/analyze-daily-bhav-data", null);
      response.EnsureSuccessStatusCode();
    }
  }
}
