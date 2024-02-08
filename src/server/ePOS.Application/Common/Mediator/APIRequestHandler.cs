using ePOS.Application.Common.Contracts;
using ePOS.Shared.ValueObjects;
using MediatR;

namespace ePOS.Application.Common.Mediator;

public abstract class APIRequestHandler<TRequest> : IRequestHandler<TRequest, APIResponse>
    where TRequest : IRequest<APIResponse>
{
    protected readonly UserClaimsValue UserClaimsValue;
    
    protected APIRequestHandler(IUserService userService)
    {
        UserClaimsValue = userService.GetUserClaimsValue();
    }

    public abstract Task<APIResponse> Handle(TRequest request, CancellationToken cancellationToken);

}

public abstract class APIRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, APIResponse<TResponse>>
    where TRequest : IRequest<APIResponse<TResponse>>
{
    protected readonly UserClaimsValue UserClaimsValue;
    
    protected APIRequestHandler(IUserService userService)
    {
        UserClaimsValue = userService.GetUserClaimsValue();
    }
    
    public abstract Task<APIResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}