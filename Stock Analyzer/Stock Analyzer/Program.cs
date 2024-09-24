using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stock_Analyzer;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Repository;
using Stock_Analyzer_Repository.Repository;
using Stock_Analyzer_Service;
using Stock_Analyzer_Service.Interface;
using Stock_Analyzer_Service.ScrapperJob;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

ScrapperJob myJob = new ScrapperJob();
ScrapperScheduler myScheduler = new ScrapperScheduler(myJob);

// Start the scheduler when the application starts
myScheduler.Start();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<StockAnalyzerContext>((services, options) =>
{
  var appSettings = services.GetRequiredService<IOptions<AppSettings>>().Value;
  options.UseSqlServer(appSettings.DatabaseConnection);
});

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularOrigins", cpb =>
  {
    var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>();

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

app.UseHttpsRedirection();

app.UseCors("AllowAngularOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
