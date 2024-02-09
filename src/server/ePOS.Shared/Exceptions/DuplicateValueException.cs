namespace ePOS.Shared.Exceptions;

public class DuplicateValueException : BadRequestException
{
    public DuplicateValueException(string attribute) : base($"Duplicate{attribute}")
    {
        
    }
    
    public DuplicateValueException(string attribute, dynamic value) : base($"Duplicate{attribute}:{value}")
    {
        
    }
}