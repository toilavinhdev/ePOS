using AutoMapper;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.ItemAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Queries;

public class GetItemQuery : IAPIRequest<ItemViewModel>
{
    public Guid Id { get; set; }
}

public class GetItemQueryValidator : AbstractValidator<GetItemQuery>
{
    public GetItemQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetItemQueryHandler : APIRequestHandler<GetItemQuery, ItemViewModel>
{
    private readonly ITenantContext _context;
    private readonly IMapper _mapper;
    
    public GetItemQueryHandler(IUserService userService, IMapper mapper, ITenantContext context) : base(userService)
    {
        _mapper = mapper;
        _context = context;
    }

    public override async Task<APIResponse<ItemViewModel>> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .Include(x => x.Unit)
            .Include(x => x.ItemImages)
            .Include(x => x.ItemSizes)
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == UserClaimsValue.TenantId, cancellationToken);

        if (item is null) throw new RecordNotFoundException(nameof(Item));

        var data = _mapper.Map<ItemViewModel>(item);
        data.Images = item.ItemImages!.Select(x => _mapper.Map<ItemImageViewModel>(x)).ToList();
        data.SizePrices = item.ItemSizes!.Select(x => _mapper.Map<ItemSizePriceViewModel>(x)).ToList();

        return new APIResponse<ItemViewModel>().IsSuccess(data);
    }
}