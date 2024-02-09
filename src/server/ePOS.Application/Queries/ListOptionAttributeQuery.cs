using System.Linq.Expressions;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.OptionAttributeAggregate;
using ePOS.Shared.Extensions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Queries;

public class ListOptionAttributeQuery : IAPIRequest<ListOptionAttributeResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public string? Name { get; set; }
    
    public bool? IsActive { get; set; }
    
    public string? Sort { get; set; }
}

public class ListOptionAttributeResponse
{
    public List<OptionAttributeViewModel> Records { get; set; } = default!;

    public Paginator Paginator { get; set; } = default!;
} 

public class ListOptionAttributeQueryValidator : AbstractValidator<ListOptionAttributeQuery>
{
    public ListOptionAttributeQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty();
        RuleFor(x => x.PageSize).NotEmpty();
    }
}

public class ListOptionAttributeQueryHandler : APIRequestHandler<ListOptionAttributeQuery, ListOptionAttributeResponse>
{
    private readonly ITenantContext _context;
    
    public ListOptionAttributeQueryHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse<ListOptionAttributeResponse>> Handle(ListOptionAttributeQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<OptionAttribute, bool>> whereExpression = x => true;
        Expression<Func<OptionAttribute, object>> sortExpression = x => x.CreatedAt;
        var sortAsc = true;

        whereExpression = whereExpression.And(x => x.TenantId == UserClaimsValue.TenantId);

        if (request.Name is not null) whereExpression = whereExpression.And(x => x.Name.Contains(request.Name));

        if (request.IsActive is not null) whereExpression = whereExpression.And(x => x.IsActive == request.IsActive);

        if (request.Sort is not null)
        {
            if (request.Sort.StartsWith("+")) sortAsc = true;
            if (request.Sort.StartsWith("-")) sortAsc = false;
            if (request.Sort.Contains("name")) sortExpression = x => x.Name;
            if (request.Sort.Contains("isActive")) sortExpression = x => x.IsActive;
            if (request.Sort.Contains("createdAt")) sortExpression = x => x.CreatedAt;
        }

        var query = await _context.OptionAttributes
            .Include(x => x.OptionAttributeValues)
            .Include(x => x.ItemOptionAttributes)
            .Where(whereExpression)
            .ToSortedQuery(sortExpression, sortAsc)
            .ToPagedQuery(request.PageIndex, request.PageSize, out var paginator)
            .ToListAsync(cancellationToken);

        var data = new ListOptionAttributeResponse()
        {
            Paginator = paginator,
            Records = query.Select(x => new OptionAttributeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    Attributes = x.OptionAttributeValues.Select(att => att.Name).ToArray(),
                    ItemCount = x.ItemOptionAttributes!.Count
                })
                .ToList()
        };

        return new APIResponse<ListOptionAttributeResponse>().IsSuccess(data);
    }
}