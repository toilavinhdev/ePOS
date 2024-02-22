using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.FileAggregate;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ePOS.Application.Commands;

public class UploadFilesCommand : IAPIRequest<List<string>>
{
    public List<IFormFile> Files { get; set; } = default!;
    
    public string? Bucket { get; set; }
}

public class UploadFilesCommandValidator : AbstractValidator<UploadFilesCommand>
{
    public UploadFilesCommandValidator()
    {
        RuleFor(x => x.Files).NotEmpty();
    }
}

public class UploadFilesCommandHandler : APIRequestHandler<UploadFilesCommand, List<string>>
{
    private readonly ITenantContext _context;
    private readonly IStorageService _storageService;
    
    public UploadFilesCommandHandler(IUserService userService, ITenantContext context, 
        IStorageService storageService) : base(userService)
    {
        _context = context;
        _storageService = storageService;
    }

    public override async Task<APIResponse<List<string>>> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {
        var fileEntities = new List<ApplicationFile>();

        var count = 0;
        
        foreach (var file in request.Files)
        {
            var sourceName = file.FileName;
            var fileName = $"{Guid.NewGuid().ToString("N").ToLower()}-[{count}]{Path.GetExtension(sourceName)}";
            
            var url = await _storageService.UploadAsync(file, "Images", fileName, cancellationToken);

            var fileEntity = new ApplicationFile(
                file.FileName, 
                fileName, 
                file.Length, 
                file.ContentType, 
                url);
            fileEntity.SetCreationTracking(UserClaimsValue);
            
            fileEntities.Add(fileEntity);
            count++;
        }
        
        await _context.Files.AddRangeAsync(fileEntities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new APIResponse<List<string>>()
            .IsSuccess(fileEntities.Select(x => x.Url).ToList(), $"Tải lên {count} file thành công");
    }
}