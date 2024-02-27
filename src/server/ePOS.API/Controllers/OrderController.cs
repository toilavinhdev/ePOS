using ePOS.Application.Commands;
using ePOS.Infrastructure.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ePOS.API.Controllers;

[ApiController]
[Route("api/v1/order")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("checkout")]
    [ApplicationPermission]
    public async Task<IActionResult> Checkout([FromBody] CreateOrderCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}