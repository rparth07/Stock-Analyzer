using Microsoft.EntityFrameworkCore;
using Stock_Analyzer_Domain.Models;
using Stock_Analyzer_Repository.DataModels;

namespace Stock_Analyzer_Repository
{
    public class StockAnalyzerContext : DbContext
    {
        public DbSet<CompanyDataModel> Company { get; set; }
        public DbSet<BhavCopyInfoDataModel> BhavCopyInfo { get; set; }
        public DbSet<ClientDataModel> Client { get; set; }
        public DbSet<BulkDealDataModel> BulkDeal { get; set; }

        public StockAnalyzerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyDataModel>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Symbol).IsUnique();
            });

            modelBuilder.Entity<BhavCopyInfoDataModel>(entity => {
                entity.HasKey(e => e.Id);
            });
            
            modelBuilder.Entity<ClientDataModel>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
            });
            //[TODO]: Do we need any unique identifier here, or we can just work in service to not allow inserting the data if for that date data is already available?
            modelBuilder.Entity<BulkDealDataModel>(entity => {
                entity.HasKey(e => e.Id);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.");
            }
        }
    }
}