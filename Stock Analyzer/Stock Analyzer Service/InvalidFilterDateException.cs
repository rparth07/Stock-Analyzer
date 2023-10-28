using System.Runtime.Serialization;

namespace Stock_Analyzer_Service
{
  [Serializable]
  internal class InvalidFilterDateException : Exception
  {
    public InvalidFilterDateException()
    {
    }

    public InvalidFilterDateException(string? message) : base(message)
    {
    }

    public InvalidFilterDateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidFilterDateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}