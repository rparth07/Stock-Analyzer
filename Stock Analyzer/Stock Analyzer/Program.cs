using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Stock_Analyzer.MarketData;
using Stock_Analyzer;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Repository;
using Stock_Analyzer_Repository.Repository;
using Stock_Analyzer_Service;
using Stock_Analyzer_Service.Interface;
using Stock_Analyzer_Service.ScrapperJob;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.MaxDepth = 32);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IBhavInfoRepository, BhavInfoRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IBulkDealRepository, BulkDealRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IFilterRepository, FilterRepository>();
builder.Services.AddScoped<INotebookRepository, NotebookRepository>();

builder.Services.AddScoped<IStockInfoService, StockInfoService>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<INotebookService, NotebookService>();

builder.Services.AddHttpClient<NseArchiveImporter>(client => { client.Timeout = TimeSpan.FromSeconds(30); });
builder.Services.AddHostedService<MarketRefreshService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Keep environment variables and command-line overrides above appsettings.json.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<StockAnalyzerContext>((services, options) =>
{
  var raw = builder.Configuration["DATABASE_URL"] ?? builder.Configuration["AppSettings:DatabaseConnection"] ?? "Host=localhost;Database=stock_analyzer";
  options.UseNpgsql(DatabaseConnection.Parse(raw));
});

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularOrigins", cpb =>
  {
    var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

    foreach (var origin in allowedOrigins)
    {
      cpb.WithOrigins(origin).AllowAnyHeader().AllowAnyMethod();
    }
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// TLS terminates at Render. Do not redirect internal HTTP health checks.
app.UseExceptionHandler(handler => handler.Run(async context => {
  context.Response.StatusCode = 503;
  await context.Response.WriteAsJsonAsync(new { message = "The data service could not complete this request. Try again later." });
}));

app.UseCors("AllowAngularOrigins");

app.UseAuthorization();

app.MapControllers();
app.MapGet("/health/ready", async (StockAnalyzerContext db) =>
  await db.Database.CanConnectAsync() ? Results.Ok(new { status = "ready" }) : Results.StatusCode(503));
if (builder.Configuration.GetValue<bool>("MigrateOnStartup")) {
  var direct = builder.Configuration["MIGRATION_DATABASE_URL"] ?? builder.Configuration["DATABASE_URL"] ?? throw new InvalidOperationException("Database configuration is required for migrations.");
  var options = new DbContextOptionsBuilder<StockAnalyzerContext>().UseNpgsql(DatabaseConnection.Parse(direct)).Options;
  await using var migrationDb = new StockAnalyzerContext(options);
  await migrationDb.Database.MigrateAsync();
}

app.Run();
