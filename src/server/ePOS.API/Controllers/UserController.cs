using ePOS.Application.Commands.User;
using ePOS.Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ePOS.API.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        return Ok(await _mediator.Send(new GetMeQuery()));
    }
    
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}