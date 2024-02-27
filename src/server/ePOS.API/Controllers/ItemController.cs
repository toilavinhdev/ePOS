using ePOS.Application.Commands;
using ePOS.Application.Queries;
using ePOS.Infrastructure.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ePOS.API.Controllers;

[ApiController]
[Route("api/v1/item")]
public class ItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ApplicationPermission]
    public async Task<IActionResult> Get([FromQuery] GetItemQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpPost("list")]
    [ApplicationPermission]
    public async Task<IActionResult> List([FromBody] ListItemQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
    
    [HttpPost("create")]
    [ApplicationPermission]
    public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost("delete")]
    [ApplicationPermission]
    public async Task<IActionResult> Delete([FromBody] DeleteItemsCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}