using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.TenantAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.Utilities;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands.User;

public class SignUpCommand : IAPIRequest<SignUpResponse>
{
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string TenantName { get; set; } = default!;
}

public class SignUpResponse
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
}

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().Matches(RegexUtils.EmailRegex);
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.TenantName).NotEmpty();
    }
}

public class SignUpCommandHandler : APIRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IUserService _userService;
    private readonly ITenantContext _context;
    
    public SignUpCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _userService = userService;
        _context = context;
    }

    public override async Task<APIResponse<SignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Tenants.AnyAsync(x => x.Name.Equals(request.TenantName), cancellationToken))
            throw new DuplicateValueException(nameof(Tenant.Name));
        
        var tenant = new Tenant()
        {
            Id = Guid.NewGuid(),
            Name = request.TenantName,
            Code = Guid.NewGuid().ToString("N"),
            CreatedAt = DateTimeOffset.Now,
            TenantTax = new TenantTax
            {
                Id = Guid.NewGuid(),
                IsApplyForAllItem = false,
                IsPriceIncludeTax = false,
                Value = 0,
                CreatedAt = DateTimeOffset.Now
            }
        };
        await _context.Tenants.AddAsync(tenant, cancellationToken);
        
        var result = await _userService.SignUpAsync(request, tenant.Id, cancellationToken);

        tenant.TenantTax.CreatedBy = result.Id;
        await _context.SaveChangesAsync(cancellationToken);
        return new APIResponse<SignUpResponse>().IsSuccess(result, "Đăng ký thành công");
    }
}