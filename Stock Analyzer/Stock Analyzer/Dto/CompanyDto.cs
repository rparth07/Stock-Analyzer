
namespace Stock_Analyzer.Dto
{
    public class CompanyDto
    {
        public Guid Id { get; set; }

        public string Symbol { get; set; }

        public ICollection<StockInfoDto> BhavCopyInfos { get; set; } = new List<StockInfoDto>();

        public ICollection<BulkDealDto> BulkDeals { get; set; } = new List<BulkDealDto>();
    }
}
