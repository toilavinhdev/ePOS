using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Shared.Utilities;
using ePOS.Shared.ValueObjects;
using FluentValidation;

namespace ePOS.Application.Commands.User;

public class ChangePasswordCommand : IAPIRequest
{
    public string Email { get; set; } = default!;

    public string CurrentPassword { get; set; } = default!;

    public string NewPassword { get; set; } = default!;
}

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Matches(RegexUtils.EmailRegex);
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
    }
}

public class ChangePasswordCommandHandler : APIRequestHandler<ChangePasswordCommand>
{
    private readonly IUserService _userService;
    
    public ChangePasswordCommandHandler(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    public override async Task<APIResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        await _userService.ChangePasswordAsync(request, cancellationToken);
        return new APIResponse().IsSuccess("Thay đổi mật khẩu thành công");
    }
}