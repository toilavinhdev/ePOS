using AutoMapper;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.TenantAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class UpdateTenantCommand : IAPIRequest<TenantViewModel>
{
    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;
    
    public string? CompanyName { get; set; }
    
    public string? CompanyAddress { get; set; }
    
    public string? LogoUrl { get; set; }
    
    public string? TaxId { get; set; }
}

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateTenantCommandHandler : APIRequestHandler<UpdateTenantCommand, TenantViewModel>
{
    private readonly ITenantContext _context;
    private readonly IMapper _mapper;
    
    public UpdateTenantCommandHandler(IUserService userService, ITenantContext context, IMapper mapper) : base(userService)
    {
        _context = context;
        _mapper = mapper;
    }

    public override async Task<APIResponse<TenantViewModel>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == UserClaimsValue.TenantId, cancellationToken);
        if (tenant is null) throw new RecordNotFoundException(nameof(Tenant));
        
        if (await _context.Tenants.AnyAsync(x => x.Code == request.Code && x.Id != UserClaimsValue.TenantId, cancellationToken))
            throw new DuplicateValueException("TenantCode");

        tenant.Code = request.Code;
        tenant.Name = request.Name;
        tenant.CompanyName = request.CompanyName ?? tenant.CompanyName;
        tenant.CompanyAddress = request.CompanyAddress ?? tenant.CompanyAddress;
        tenant.LogoUrl = request.LogoUrl ?? tenant.LogoUrl;
        tenant.TaxId = request.TaxId ?? tenant.TaxId;
        tenant.ModifiedAt = DateTimeOffset.Now;

        await _context.SaveChangesAsync(cancellationToken);

        var model = _mapper.Map<TenantViewModel>(tenant);
        
        return new APIResponse<TenantViewModel>().IsSuccess(model, "Cập nhật thương hiệu thành công");
    }
}