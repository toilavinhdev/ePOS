using AutoMapper;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.UnitAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class UpdateUnitCommand : IAPIRequest<UnitViewModel>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
}

public class UpdateUnitCommandValidator : AbstractValidator<UpdateUnitCommand>
{
    public UpdateUnitCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateUnitCommandHandler : APIRequestHandler<UpdateUnitCommand, UnitViewModel>
{
    private readonly ITenantContext _context;
    private readonly IMapper _mapper;
    
    public UpdateUnitCommandHandler(IUserService userService, ITenantContext context, IMapper mapper) : base(userService)
    {
        _context = context;
        _mapper = mapper;
    }

    public override async Task<APIResponse<UnitViewModel>> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await _context.Units.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit is null) throw new RecordNotFoundException(nameof(Unit));

        unit.Name = request.Name;
        unit.SetModificationTracking(UserClaimsValue);

        await _context.SaveChangesAsync(cancellationToken);

        var data = _mapper.Map<UnitViewModel>(unit);

        return new APIResponse<UnitViewModel>().IsSuccess(data, "Cập nhật đơn vị thành công");
    }
}