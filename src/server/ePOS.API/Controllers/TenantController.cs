using ePOS.Application.Commands;
using ePOS.Application.Queries;
using ePOS.Infrastructure.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ePOS.API.Controllers;

[ApiController]
[Route("api/v1/tenant")]
public class TenantController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ApplicationPermission]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new GetTenantQuery()));
    }

    [HttpPut("update")]
    [ApplicationPermission]
    public async Task<IActionResult> Update([FromBody] UpdateTenantCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}