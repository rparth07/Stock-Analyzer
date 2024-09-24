namespace Stock_Analyzer.Dto
{
  public class ClientDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<BulkDealDto> Deals { get; set; } = new List<BulkDealDto>();
  }
}
