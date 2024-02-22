using ePOS.Application.Commands;
using ePOS.Application.Queries;
using ePOS.Shared.ValueObjects;

namespace ePOS.Application.Common.Contracts;

public interface IUserService
{
    UserClaimsValue GetUserClaimsValue();
    Task<SignInResponse> SignInAsync(SignInCommand command, CancellationToken cancellationToken);
    Task<SignUpResponse> SignUpAsync(SignUpCommand command, Guid tenantId, CancellationToken cancellationToken);
    Task<GetMeResponse> GetMeAsync(Guid id, CancellationToken cancellationToken);
    Task ChangePasswordAsync(ChangePasswordCommand command, CancellationToken cancellationToken);
}