using System.Linq.Expressions;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.ItemAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.Extensions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Queries;

public class ListItemQuery : IAPIRequest<ListItemResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public string? Name { get; set; }
    
    public Guid? CategoryId { get; set; }
    
    public bool? IsActive { get; set; }
    
    public string? Sort { get; set; }
}

public class ListItemResponse
{
    public List<ItemViewModel> Records { get; set; } = default!;

    public Paginator Paginator { get; set; } = default!;
}

public class ListItemQueryValidator : AbstractValidator<ListItemQuery>
{
    public ListItemQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty();
        RuleFor(x => x.PageSize).NotEmpty();
    }
}

public class ListItemQueryHandler : APIRequestHandler<ListItemQuery, ListItemResponse>
{
    private readonly ITenantContext _context;
    
    public ListItemQueryHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse<ListItemResponse>> Handle(ListItemQuery request, CancellationToken cancellationToken)
    {
        if (request.CategoryId is not null &&
            !await _context.Categories.AnyAsync(x => x.Id.Equals(request.CategoryId), cancellationToken))
            throw new RecordNotFoundException(nameof(Category));

        Expression<Func<Item, bool>> whereExpression = x => true;
        Expression<Func<Item, object>> sortExpression = x => x.CreatedAt;
        var sortAsc = true;
        
        whereExpression = whereExpression.And(x => x.TenantId.Equals(UserClaimsValue.TenantId));

        if (request.Name is not null) whereExpression = whereExpression.And(x => x.Name.Contains(request.Name));

        if (request.CategoryId is not null) whereExpression = whereExpression.And(x => x.CategoryItems!
            .Any(categoryItem => categoryItem.CategoryId.Equals(request.CategoryId)));

        if (request.IsActive is not null)
            whereExpression = whereExpression.And(x => x.IsActive.Equals(request.IsActive));

        if (request.Sort is not null)
        {
            if (request.Sort.StartsWith("+")) sortAsc = true;
            if (request.Sort.StartsWith("-")) sortAsc = false;
            if (request.Sort.Contains("name")) sortExpression = x => x.Name;
            if (request.Sort.Contains("createdAt")) sortExpression = x => x.CreatedAt;
            if (request.Sort.Contains("isActive")) sortExpression = x => x.IsActive;
        }

        var query = await _context.Items
            .Include(x => x.CategoryItems)
            .Include(x => x.ItemImages)
            .Include(x => x.Unit)
            .Include(x => x.ItemSizes)
            .Include(x => x.ItemOptionAttributes)!
                .ThenInclude(x => x.OptionAttribute)
                    .ThenInclude(x => x.OptionAttributeValues)
            .Include(x => x.ItemToppings)!
                .ThenInclude(x => x.Topping)
            .Include(x => x.CategoryItems)!
                .ThenInclude(x => x.Category)
            .Where(whereExpression)
            .ToSortedQuery(sortExpression, sortAsc)
            .ToPagedQuery(request.PageIndex, request.PageSize, out var paginator)
            .ToListAsync(cancellationToken);

        var result = new ListItemResponse()
        {
            Records = query.Select(x => new ItemViewModel()
            {
                Id = x.Id,
                Sku = x.Sku,
                Name = x.Name,
                IsActive = x.IsActive,
                Price = x.Price,
                SizePrices = x.ItemSizes?.Select(y => new ItemSizePriceViewModel()
                {
                    Name = y.Name,
                    Price = y.Price,
                }).ToList() ?? new List<ItemSizePriceViewModel>(),
                UnitId = x.UnitId,
                UnitName = x.Unit.Name,
                Images = x.ItemImages?.Select(y => new ItemImageViewModel()
                {
                    SortIndex = y.SortIndex,
                    Url = y.Url
                }).ToList(),
                OptionAttributes = x.ItemOptionAttributes?
                    .Select(y => y.OptionAttribute)
                    .Select(y => new OptionAttributeViewModel()
                    {
                        Id = y.Id,
                        Name = y.Name,
                        IsActive = y.IsActive,
                        ItemCount = y.ItemOptionAttributes?.Count ?? 0,
                        Attributes = y.OptionAttributeValues.Select(z => z.Name).ToArray()
                    }).ToList() ?? new List<OptionAttributeViewModel>(),
                Toppings = x.ItemToppings?
                    .Select(y => y.Topping)
                    .Select(y => new ToppingViewModel()
                    {
                        Id = y.Id,
                        Name = y.Name,
                        IsActive = y.IsActive,
                        Price = y.Price,
                        ItemCount = y.ItemToppings?.Count ?? 0
                    }).ToList() ?? new List<ToppingViewModel>(), 
                ItemCategories = x.CategoryItems?
                    .Select(y => y.Category)
                    .Select(y => new ItemCategoryViewModel()
                    {
                        Id = y.Id,
                        Name = y.Name
                    }).ToList() ?? new List<ItemCategoryViewModel>(),
                CreatedAt = x.CreatedAt
            }).ToList(),
            Paginator = paginator
        };

        return new APIResponse<ListItemResponse>().IsSuccess(result);
    }
}