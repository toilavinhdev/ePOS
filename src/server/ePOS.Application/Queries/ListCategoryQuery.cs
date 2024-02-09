using System.Linq.Expressions;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.CategoryAggregate;
using ePOS.Shared.Extensions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Queries;

public class ListCategoryQuery : IAPIRequest<ListCategoryResponse>
{
    public string? Name { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
    
    public string? Sort { get; set; }
    
    public bool? IsActive { get; set; }
}

public class ListCategoryQueryValidator : AbstractValidator<ListCategoryQuery>
{
    public ListCategoryQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty();
        RuleFor(x => x.PageSize).NotEmpty();
    }
}

public class ListCategoryResponse
{
    public List<CategoryViewModel> Records { get; set; } = default!;

    public Paginator Paginator { get; set; } = default!;
}

public class ListCategoryQueryHandler : APIRequestHandler<ListCategoryQuery, ListCategoryResponse>
{
    private readonly ITenantContext _context;
    
    public ListCategoryQueryHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override Task<APIResponse<ListCategoryResponse>> Handle(ListCategoryQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Category, bool>> whereExpression = x => true;
        Expression<Func<Category, object>> sortExpression = x => x.CreatedAt;
        var sortAsc = true;
        
        whereExpression.And(x => x.TenantId.Equals(UserClaimsValue.TenantId));

        if (request.Name is not null) whereExpression = whereExpression.And(x => x.Name.Contains(request.Name));
        
        if (request.IsActive is not null) whereExpression = whereExpression.And(x => x.IsActive == request.IsActive);
        
        if (request.Sort is not null)
        {
            if (request.Sort.StartsWith("+")) sortAsc = true;
            if (request.Sort.StartsWith("-")) sortAsc = false;
            if (request.Sort.Contains("name")) sortExpression = x => x.Name;
            if (request.Sort.Contains("createdAt")) sortExpression = x => x.CreatedAt;
            if (request.Sort.Contains("isActive")) sortExpression = x => x.IsActive;
        }

        var query = _context.Categories
            .Include(x => x.CategoryItems)
            .Where(whereExpression)
            .ToSortedQuery(sortExpression, sortAsc)
            .ToPagedQuery(request.PageIndex, request.PageSize, out var paginator);

        var result = new ListCategoryResponse()
        {
            Paginator = paginator,
            Records = query.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                ItemCount = x.CategoryItems!.Count
            }).ToList()
        };

        return Task.FromResult(new APIResponse<ListCategoryResponse>().IsSuccess(result));
    }
}