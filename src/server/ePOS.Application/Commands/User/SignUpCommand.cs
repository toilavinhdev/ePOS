using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Shared.Utilities;
using ePOS.Shared.ValueObjects;
using FluentValidation;

namespace ePOS.Application.Commands.User;

public class SignUpCommand : IAPIRequest<SignUpResponse>
{
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}

public class SignUpResponse
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
}

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().Matches(RegexUtils.EmailRegex);
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class SignUpCommandHandler : APIRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IUserService _userService;
    
    public SignUpCommandHandler(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    public override async Task<APIResponse<SignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var result = await _userService.SignUpAsync(request, cancellationToken);
        return new APIResponse<SignUpResponse>().IsSuccess(result, "Đăng ký thành công");
    }
}