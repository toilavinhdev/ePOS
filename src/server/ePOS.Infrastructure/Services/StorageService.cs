using ePOS.Application.Common.Contracts;
using ePOS.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace ePOS.Infrastructure.Services;

public class StorageService : IStorageService
{
    private readonly string _host;
    private readonly StorageConfig _storageConfig;

    public StorageService(AppSettings appSettings)
    {
        _host = appSettings.Host;
        _storageConfig = appSettings.StorageConfig;
    }

    public async Task<string> UploadAsync(IFormFile file, string? bucket, string fileName, CancellationToken cancellationToken)
    {
        var rootBucketFullPath = _storageConfig.Location;

        if (bucket is not null) rootBucketFullPath = Path.Combine(rootBucketFullPath, bucket);

        InitialBucket(rootBucketFullPath);
        
        var fileFullPath = Path.Combine(rootBucketFullPath, fileName);
        
        await using var fileStream = new FileStream(fileFullPath, FileMode.Create);
        
        await file.CopyToAsync(fileStream, cancellationToken);
        
        var hostBucket = $"{_host}{_storageConfig.ExternalPath}";
        
        return bucket is not null ? $"{hostBucket}/{bucket}/{fileName}" : $"{hostBucket}/{fileName}";
    }

    private string InitialBucket(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    }
}