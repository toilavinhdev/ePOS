using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.FileAggregate;

public class ApplicationFile : AuditableEntity
{
    public string SourceName { get; init; }

    public string FileName { get; init; }
    
    public long Size { get; init; }

    public string ContentType { get; init; }

    public string Url { get; init; }

    public ApplicationFile (string sourceName, string fileName, long size, string contentType, string url)
    {
        Id = Guid.NewGuid();
        SourceName = sourceName;
        FileName = fileName;
        Size = size;
        ContentType = contentType;
        Url = url;
    }
}