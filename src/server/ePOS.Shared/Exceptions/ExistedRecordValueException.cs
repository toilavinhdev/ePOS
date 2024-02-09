namespace ePOS.Shared.Exceptions;

public class ExistedRecordValueException : BadRequestException
{
    public ExistedRecordValueException(string recordName, string attributeName, string value) : base($"{recordName}AlreadyExists{attributeName}:{value}")
    {
        
    }
}