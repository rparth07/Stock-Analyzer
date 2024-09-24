using CsvHelper;
using System.Globalization;

namespace Stock_Analyzer.Helper
{
  public static class DataParsingHelper
  {
    public static List<T> ParseData<T>(this T dataObject, IFormFile postedFile)
    {
      try
      {
        List<T> parsedData = new List<T>();

        using (StreamReader streamReader = new StreamReader(postedFile.OpenReadStream()))
        {
          var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
          parsedData = csvReader.GetRecords<T>().ToList();
        }
        return parsedData;
      }
      catch (Exception ex)
      {
        throw new Exception($"Can not proccess File Please check and Try again. error: {ex.Message}");
      }
    }

    public static List<T> ParseData<T>(this T dataObject, StreamReader streamReader)
    {
      try
      {
        List<T> parsedData = new List<T>();

        var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
        parsedData = csvReader.GetRecords<T>().ToList();

        return parsedData;
      }
      catch (Exception ex)
      {
        throw new Exception($"Can not proccess File Please check and Try again. error: {ex.Message}");
      }
    }

    public static List<T> ParseData<T>(this T dataObject, string path)
    {
      try
      {
        List<T> parsedData = new List<T>();

        using (StreamReader streamReader = new StreamReader(path))
        {
          var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
          parsedData = csvReader.GetRecords<T>().ToList();
        }
        return parsedData;
      }
      catch (Exception ex)
      {
        throw new Exception($"Can not proccess File Please check and Try again. error: {ex.Message}");
      }
    }
  }
}
