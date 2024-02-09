using System.Linq.Expressions;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.ToppingAggregate;
using ePOS.Shared.Extensions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Queries;

public class ListToppingQuery : IAPIRequest<ListToppingResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public string? Name { get; set; }
    
    public bool? IsActive { get; set; }
    
    public string? Sort { get; set; }
}

public class ListToppingResponse
{
    public List<ToppingViewModel> Records { get; set; } = default!;

    public Paginator Paginator { get; set; } = default!;
}

public class ListToppingQueryValidator : AbstractValidator<ListToppingQuery>
{
    public ListToppingQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty();
        RuleFor(x => x.PageSize).NotEmpty();
    }
}

public class ListToppingQueryHandler : APIRequestHandler<ListToppingQuery, ListToppingResponse>
{
    private readonly ITenantContext _context;
    
    public ListToppingQueryHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse<ListToppingResponse>> Handle(ListToppingQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Topping, bool>> whereExpression = x => true;
        Expression<Func<Topping, object>> sortExpression = x => x.CreatedAt;
        var sortAsc = true;

        whereExpression = whereExpression.And(x => x.TenantId == UserClaimsValue.TenantId);

        if (request.Name is not null) whereExpression = whereExpression.And(x => x.Name.Contains(request.Name));

        if (request.IsActive is not null) whereExpression = whereExpression.And(x => x.IsActive == request.IsActive);

        if (request.Sort is not null)
        {
            if (request.Sort.StartsWith("+")) sortAsc = true;
            if (request.Sort.StartsWith("-")) sortAsc = false;
            if (request.Sort.Contains("name")) sortExpression = x => x.Name;
            if (request.Sort.Contains("price")) sortExpression = x => x.Price;
            if (request.Sort.Contains("isActive")) sortExpression = x => x.IsActive;
            if (request.Sort.Contains("createdAt")) sortExpression = x => x.CreatedAt;
        }

        var query = await _context.Toppings
            .Include(x => x.ItemToppings)
            .Where(whereExpression)
            .ToSortedQuery(sortExpression, sortAsc)
            .ToPagedQuery(request.PageIndex, request.PageSize, out var paginator)
            .ToListAsync(cancellationToken);

        var data = new ListToppingResponse()
        {
            Paginator = paginator,
            Records = query.Select(x => new ToppingViewModel
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Name = x.Name,
                Price = x.Price,
                ItemCount = x.ItemToppings!.Count
            }).ToList()
        };

        return new APIResponse<ListToppingResponse>().IsSuccess(data);
    }
}