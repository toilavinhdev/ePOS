using ePOS.Application.Commands;
using ePOS.Application.Queries;
using ePOS.Infrastructure.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ePOS.API.Controllers;

[ApiController]
[Route("api/v1/unit")]
public class UnitController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("list")]
    [ApplicationPermission]
    public async Task<IActionResult> List([FromBody] ListUnitQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpPost("create")]
    [ApplicationPermission]
    public async Task<IActionResult> Create([FromBody] CreateUnitCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}