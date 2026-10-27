using Npgsql;
namespace Stock_Analyzer;
public static class DatabaseConnection {
 public static string Parse(string raw) {
  if (!raw.StartsWith("postgres")) return raw;
  var uri = new Uri(raw); var credentials = uri.UserInfo.Split(':', 2);
  return new NpgsqlConnectionStringBuilder {
    Host = uri.Host, Port = uri.Port > 0 ? uri.Port : 5432, Database = uri.AbsolutePath.Trim('/'),
    Username = Uri.UnescapeDataString(credentials[0]), Password = Uri.UnescapeDataString(credentials[1]),
    SslMode = SslMode.VerifyFull, Timeout = 15, CommandTimeout = 60, MaxPoolSize = 10
  }.ConnectionString;
 }
}
