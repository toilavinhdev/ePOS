using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.ToppingAggregate;
using ePOS.Shared.ValueObjects;
using FluentValidation;

namespace ePOS.Application.Commands;

public class CreateToppingCommand : IAPIRequest
{
    public string Name { get; set; } = default!;
    
    public double Price { get; set; }
}

public class CreateToppingCommandValidator : AbstractValidator<CreateToppingCommand>
{
    public CreateToppingCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
    }
}

public class CreateToppingCommandHandler : APIRequestHandler<CreateToppingCommand>
{
    private readonly ITenantContext _context;
    
    public CreateToppingCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse> Handle(CreateToppingCommand request, CancellationToken cancellationToken)
    {
        var topping = new Topping()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            IsActive = true
        };
        topping.SetCreationTracking(UserClaimsValue);

        await _context.Toppings.AddAsync(topping, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new APIResponse().IsSuccess("Tạo topping thành công");
    }
}