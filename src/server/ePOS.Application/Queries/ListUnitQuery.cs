using System.Linq.Expressions;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.UnitAggregate;
using ePOS.Shared.Extensions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Queries;

public class ListUnitQuery : IAPIRequest<ListUnitResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public string? Name { get; set; }
    
    public bool? IsDefault { get; set; }
    
    public string? Sort { get; set; }
}

public class ListUnitResponse
{
    public List<UnitViewModel> Records { get; set; } = default!;

    public Paginator Pagination { get; set; } = default!;
}

public class ListUnitQueryValidator : AbstractValidator<ListUnitQuery>
{
    public ListUnitQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty();
        RuleFor(x => x.PageSize).NotEmpty();
    }
}

public class ListUnitQueryHandler : APIRequestHandler<ListUnitQuery, ListUnitResponse>
{
    private readonly ITenantContext _context;
    
    public ListUnitQueryHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override Task<APIResponse<ListUnitResponse>> Handle(ListUnitQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Unit, bool>> whereExpression = x => true;
        Expression<Func<Unit, object>> sortExpression = x => x.CreatedAt;
        var sortAsc = true;
        
        whereExpression = whereExpression.And(x => x.TenantId.Equals(Guid.Empty) || x.TenantId.Equals(UserClaimsValue.TenantId));
        
        if (request.Name is not null) whereExpression = whereExpression.And(x => x.Name.Contains(request.Name));

        if (request.IsDefault is not null) whereExpression = whereExpression.And(x => x.IsDefault == request.IsDefault);
        
        if (request.Sort is not null)
        {
            if (request.Sort.StartsWith("+")) sortAsc = true;
            if (request.Sort.StartsWith("-")) sortAsc = false;
            if (request.Sort.Contains("name")) sortExpression = x => x.Name;
            if (request.Sort.Contains("createdAt")) sortExpression = x => x.CreatedAt;
        }

        var query = _context.Units
            .Include(x => x.Items)
            .Where(whereExpression)
            .ToSortedQuery(sortExpression, sortAsc)
            .ToPagedQuery(request.PageIndex, request.PageSize, out var pagination);

        var result = new ListUnitResponse()
        {
            Records = query.Select(x => new UnitViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsDefault = x.IsDefault,
                ItemCount = x.Items!.Count
            }).ToList(),
            Pagination = pagination
        };

        return Task.FromResult(new APIResponse<ListUnitResponse>().IsSuccess(result));
    }
}