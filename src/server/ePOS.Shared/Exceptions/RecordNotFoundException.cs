namespace ePOS.Shared.Exceptions;

public class RecordNotFoundException : BadRequestException
{
    public RecordNotFoundException(string recordName) : base( $"{recordName}NotFound")
    {
        
    }
    
    public RecordNotFoundException(string recordName, Guid? recordId = null) : base( $"{recordName}NotFound {recordId}")
    {
        
    }
}