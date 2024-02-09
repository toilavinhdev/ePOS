﻿using System.Linq.Expressions;
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

    public Paginator Pagination { get; set; } = default!;
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
            if (request.Sort.Contains("createdAt")) sortExpression = x => x.CreatedAt;
        }

        var query = _context.Items
            .Include(x => x.CategoryItems)
            .Include(x => x.ItemImages)
            .Include(x => x.Unit)
            .Where(whereExpression)
            .ToSortedQuery(sortExpression, sortAsc)
            .ToPagedQuery(request.PageIndex, request.PageSize, out var pagination);

        var result = new ListItemResponse()
        {
            Records = query.Select(x => new ItemViewModel()
            {
                Id = x.Id,
                Sku = x.Sku,
                Name = x.Name,
                IsActive = x.IsActive,
                Price = x.Price,
                SizePrices = x.ItemSizes!.Select(y => new ItemSizePriceViewModel()
                {
                    Name = y.Name,
                    Price = y.Price,
                }).ToList(),
                UnitId = x.UnitId,
                UnitName = x.Unit.Name,
                Images = x.ItemImages!.Select(y => new ItemImageViewModel()
                {
                    SortIndex = y.SortIndex,
                    Url = y.Url
                }).ToList(),
                CreatedAt = x.CreatedAt
            }).ToList(),
            Pagination = pagination
        };

        return new APIResponse<ListItemResponse>().IsSuccess(result);
    }
}