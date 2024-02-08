using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.FileAggregate;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ePOS.Application.Commands.Storage;

public class UploadFileCommand : IAPIRequest<string>
{
    public IFormFile File { get; set; } = default!;
    
    public string? Bucket { get; set; }
}

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File).NotEmpty();
    }
}

public class UploadFileCommandHandler : APIRequestHandler<UploadFileCommand, string>
{
    private readonly ITenantContext _context;
    private readonly IStorageService _storageService;
    
    public UploadFileCommandHandler(IUserService userService, ITenantContext context, 
        IStorageService storageService) : base(userService)
    {
        _context = context;
        _storageService = storageService;
    }

    public override async Task<APIResponse<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var sourceName = request.File.FileName;
        var fileName = $"{Guid.NewGuid().ToString("N").ToLower()}{Path.GetExtension(sourceName)}";
        var url = await _storageService.UploadAsync(request.File, request.Bucket, fileName, cancellationToken);

        var file = new ApplicationFile(
            request.File.FileName, 
            fileName, 
            request.File.Length, 
            request.File.ContentType, 
            url);
        file.SetCreationTracking(UserClaimsValue);
        
        await _context.Files.AddAsync(file, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new APIResponse<string>().IsSuccess(file.Url, "Upload thành công");
    }
}