﻿using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.ItemAggregate;
using ePOS.Domain.UnitAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class CreateItemCommand : IAPIRequest
{
    public string Name { get; set; } = default!;

    public string Sku { get; set; } = default!;
    
    public double Price { get; set; }
    
    public Guid UnitId { get; set; }

    public string[]? ImageUrls { get; set; }
    
    public List<ItemSizePriceRequest>? SizePrices { get; set; }
}

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Sku).NotEmpty();
        RuleFor(x => x.UnitId).NotEmpty();
    }
}

public class CreateItemCommandHandler : APIRequestHandler<CreateItemCommand>
{
    private readonly ITenantContext _context;
    
    public CreateItemCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Units.AnyAsync(x => x.Id.Equals(request.UnitId), cancellationToken))
            throw new RecordNotFoundException(nameof(Unit));
        
        if (await _context.Items.AnyAsync(x => x.Sku.Equals(request.Sku) && x.TenantId == UserClaimsValue.TenantId, cancellationToken)) 
            throw new DuplicateValueException(nameof(Item.Sku));

        var item = new Item()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsActive = true,
            Sku = request.Sku,
            UnitId = request.UnitId,
            Price = request.Price,
        };
        item.SetCreationTracking(UserClaimsValue);
        await _context.Items.AddAsync(item, cancellationToken);

        if (request.ImageUrls is not null)
        {
            var images = request.ImageUrls.Select((x, idx) => new ItemImage()
            {
                Id = Guid.NewGuid(),
                TenantId = UserClaimsValue.TenantId,
                ItemId = item.Id,
                Url = x,
                SortIndex = idx
            });
            await _context.ItemImages.AddRangeAsync(images, cancellationToken);
        }

        if (request.SizePrices is not null)
        {
            var sizes = request.SizePrices.Select((x, idx) => new ItemSize()
            {
                Id = Guid.NewGuid(),
                ItemId = item.Id,
                Name = x.Name,
                Price = x.Price,
                SortIndex = idx,
                TenantId = UserClaimsValue.TenantId,
                CreatedAt = DateTimeOffset.Now,
                CreatedBy = UserClaimsValue.Id
            });
            await _context.ItemSizes.AddRangeAsync(sizes, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        
        return new APIResponse().IsSuccess("Tạo món ăn thành công");
    }
}

public class ItemSizePriceRequest
{
    public string Name { get; set; } = default!;
    
    public double Price { get; set; }
}