namespace Stock_Analyzer_Repository;
public class ImportBatch {
 public Guid Id { get; set; }
 public string Source { get; set; } = "";
 public DateTime TradingDate { get; set; }
 public DateTime ImportedAt { get; set; }
 public int RowCount { get; set; }
}
