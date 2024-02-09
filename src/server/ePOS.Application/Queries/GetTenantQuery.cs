using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.TenantAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Queries;

public record GetTenantQuery : IAPIRequest<TenantViewModel>;

public class GetTenantQueryHandler : APIRequestHandler<GetTenantQuery, TenantViewModel>
{
    private readonly ITenantContext _context;
    
    public GetTenantQueryHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse<TenantViewModel>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == UserClaimsValue.TenantId, cancellationToken);
        if (tenant is null) throw new RecordNotFoundException(nameof(Tenant));

        var data = new TenantViewModel()
        {
            Id = Guid.NewGuid(),
            Code = tenant.Code,
            Name = tenant.Name,
            LogoUrl = tenant.LogoUrl,
            TaxId = tenant.TaxId,
            CompanyName = tenant.Name,
            CompanyAddress = tenant.CompanyAddress,
            CreatedAt = tenant.CreatedAt
        };

        return new APIResponse<TenantViewModel>().IsSuccess(data);
    }
}