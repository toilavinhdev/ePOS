using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Shared.ValueObjects;

namespace ePOS.Application.Queries;

public class GetMeQuery : IAPIRequest<GetMeResponse>
{
    
}

public class GetMeResponse
{
    public Guid Id { get; set; }

    public string? FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string? AvatarUrl { get; set; } = default!;
}

public class GetMeQueryHandler : APIRequestHandler<GetMeQuery, GetMeResponse>
{
    private readonly IUserService _userService;
    
    public GetMeQueryHandler(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    public override async Task<APIResponse<GetMeResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetMeAsync(UserClaimsValue.Id!.Value, cancellationToken);
        return new APIResponse<GetMeResponse>().IsSuccess(result);
    }
}