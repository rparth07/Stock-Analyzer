using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Repository.DataModels;
using Stock_Analyzer_Repository.DataModels.Filter;

namespace Stock_Analyzer_Repository
{
  public class StockAnalyzerContext : DbContext
  {
    public DbSet<CompanyDataModel> Company { get; set; }
    public DbSet<BhavCopyInfoDataModel> BhavCopyInfo { get; set; }
    public DbSet<ClientDataModel> Client { get; set; }
    public DbSet<BulkDealDataModel> BulkDeal { get; set; }
    public DbSet<FilterDataModel> Filter { get; set; }
    public DbSet<FilterCriteriaDataModel> FilterCriteria { get; set; }
    public DbSet<FilterResultDataModel> FilterResult { get; set; }
    public DbSet<NotebookDataModel> Notebook { get; set; }

    public StockAnalyzerContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ImportBatch> ImportBatches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      foreach (var entity in modelBuilder.Model.GetEntityTypes())
        foreach (var property in entity.GetProperties())
          if (property.ClrType == typeof(DateTime)) property.SetColumnType("date");
      modelBuilder.Entity<ImportBatch>().HasKey(x => x.Id);
      modelBuilder.Entity<ImportBatch>().HasIndex(x => new { x.Source, x.TradingDate }).IsUnique();
      modelBuilder.Entity<ImportBatch>().Property(x => x.ImportedAt).HasColumnType("timestamp with time zone");
      modelBuilder.Entity<BhavCopyInfoDataModel>().HasIndex("CompanyId", "Series", "Date").IsUnique();
      modelBuilder.Entity<CompanyDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Symbol).IsUnique();
      });

      modelBuilder.Entity<BhavCopyInfoDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
      });

      modelBuilder.Entity<ClientDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Name).IsUnique();
      });
      //[TODO]: Do we need any unique identifier here, or we can just work in service to not allow inserting the data if for that date data is already available?
      modelBuilder.Entity<BulkDealDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
      });

      modelBuilder.Entity<FilterDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.FilterName).IsUnique();
      });

      modelBuilder.Entity<FilterCriteriaDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
      });

      modelBuilder.Entity<FilterResultDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
      });

      modelBuilder.Entity<NotebookDataModel>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.ContentDate).IsUnique();
      });
    }

  }
}
