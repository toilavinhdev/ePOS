using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Shared.ValueObjects;
using FluentValidation;

namespace ePOS.Application.Commands.User;

public class SignInCommand : IAPIRequest<SignInResponse>
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}

public class SignInResponse
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class SignInCommandHandler : APIRequestHandler<SignInCommand, SignInResponse>
{
    private readonly IUserService _userService;
    
    public SignInCommandHandler(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    public override async Task<APIResponse<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.SignInAsync(request, cancellationToken);
        return new APIResponse<SignInResponse>().IsSuccess(result, "Đăng nhập thành công");
    }
}