using AutoMapper;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.UnitAggregate;
using ePOS.Shared.ValueObjects;
using FluentValidation;

namespace ePOS.Application.Commands;

public class CreateUnitCommand : IAPIRequest
{
    public string Name { get; set; } = default!;
}

public class CreateUnitCommandValidator : AbstractValidator<CreateUnitCommand>
{
    public CreateUnitCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}  

public class CreateUnitCommandCommandHandler : APIRequestHandler<CreateUnitCommand>
{
    private readonly ITenantContext _context;
    private readonly IMapper _mapper;

    public CreateUnitCommandCommandHandler(IUserService userService, ITenantContext context, IMapper mapper) : base(userService)
    {
        _context = context;
        _mapper = mapper;
    }

    public override async Task<APIResponse> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = new Unit()
        {
            Name = request.Name,
            IsDefault = false,
            TenantId = UserClaimsValue.TenantId
        };
        unit.SetCreationTracking(UserClaimsValue);
        
        await _context.Units.AddAsync(unit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var data = _mapper.Map<UnitViewModel>(unit);
        data.ItemCount = 0;
    
        return new APIResponse().IsSuccess("Tạo đơn vị thành công");
    }
}