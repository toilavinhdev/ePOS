using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.OptionAttributeAggregate;
using ePOS.Shared.ValueObjects;
using FluentValidation;

namespace ePOS.Application.Commands;

public class CreateOptionAttributeCommand : IAPIRequest
{
    public string Name { get; set; } = default!;

    public string[] Attributes { get; set; } = default!;
}

public class CreateOptionAttributeCommandValidator : AbstractValidator<CreateOptionAttributeCommand>
{
    public CreateOptionAttributeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Attributes).NotEmpty();
    }
}

public class CreateOptionAttributeCommandHandler : APIRequestHandler<CreateOptionAttributeCommand>
{
    private readonly ITenantContext _context;
    
    public CreateOptionAttributeCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse> Handle(CreateOptionAttributeCommand request, CancellationToken cancellationToken)
    {
        var option = new OptionAttribute()
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            Name = request.Name,
            OptionAttributeValues = request.Attributes.Select((x, idx) => new OptionAttributeValue()
            {
                Id = Guid.NewGuid(),
                TenantId = UserClaimsValue.TenantId,
                SortIndex = idx,
                Name = x
            }).ToList()
        };
        option.SetCreationTracking(UserClaimsValue);
        await _context.OptionAttributes.AddAsync(option, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new APIResponse().IsSuccess("Tạo tùy chọn thành công");
    }
}