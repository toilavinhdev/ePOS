namespace ePOS.Shared.Exceptions;

public class CustomException : Exception
{
    public int StatusCode { get; set; }
    
    public CustomException(int statusCode)
    {
        StatusCode = statusCode;
    }

    public CustomException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public CustomException(int statusCode, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    } 
}