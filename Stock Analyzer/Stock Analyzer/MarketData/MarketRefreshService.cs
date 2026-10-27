namespace Stock_Analyzer.MarketData;
public class MarketRefreshService(IServiceScopeFactory scopes, IConfiguration configuration, ILogger<MarketRefreshService> logger) : BackgroundService {
  protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
    if (!configuration.GetValue<bool>("RefreshMarketData")) return;
    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
    while (!stoppingToken.IsCancellationRequested) {
      var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Asia/Kolkata").Date;
      for (var days = 14; days >= 0; days--) {
        var date = today.AddDays(-days);
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
        try { using var scope = scopes.CreateScope(); await scope.ServiceProvider.GetRequiredService<NseArchiveImporter>().ImportBhavAsync(date, stoppingToken); }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { return; }
        catch (Exception ex) { logger.LogWarning("NSE bhavcopy refresh failed for {Date}: {Type}", date.ToString("yyyy-MM-dd"), ex.GetType().Name); }
      }
      try { using var scope = scopes.CreateScope(); await scope.ServiceProvider.GetRequiredService<NseArchiveImporter>().ImportBulkAsync(stoppingToken); }
      catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { return; }
      catch (Exception ex) { logger.LogWarning("NSE bulk refresh failed: {Type}", ex.GetType().Name); }
      await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
    }
  }
}
