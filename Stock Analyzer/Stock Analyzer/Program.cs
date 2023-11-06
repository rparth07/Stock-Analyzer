using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Iterface;
using Stock_Analyzer_Repository;
using Stock_Analyzer_Repository.Repository;
using Stock_Analyzer_Service;
using Stock_Analyzer_Service.Interface;

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


builder.Services.AddScoped<IStockInfoService, StockInfoService>();
builder.Services.AddScoped<IFilterService, FilterService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularOrigins",
  builder =>
  {
    builder.WithOrigins("http://localhost:4200").AllowAnyHeader();
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod();

    builder.WithOrigins("http://localhost:5052").AllowAnyHeader();
    builder.WithOrigins("http://localhost:5052").AllowAnyMethod();

    builder.WithOrigins("http://192.168.1.4:5052").AllowAnyHeader();
    builder.WithOrigins("http://192.168.1.4:5052").AllowAnyMethod();
  });
});

builder.Services.AddDbContext<StockAnalyzerContext>(options =>
{
  options.UseSqlServer(
        @"Server=LAPTOP-6RMK97C7\SQLEXPRESS; Database=StockAnalyzer; User Id=sa; password=12345; Trusted_Connection=False; TrustServerCertificate=true; MultipleActiveResultSets=true;");
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
