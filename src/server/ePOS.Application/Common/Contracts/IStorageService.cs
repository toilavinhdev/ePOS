using Microsoft.AspNetCore.Http;

namespace ePOS.Application.Common.Contracts;

public interface IStorageService
{
    Task<string> UploadAsync(IFormFile file, string? bucket, string fileName, CancellationToken cancellationToken = new());
}