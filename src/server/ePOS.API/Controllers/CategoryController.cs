﻿using ePOS.Application.Commands;
using ePOS.Application.Queries;
using ePOS.Infrastructure.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ePOS.API.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    [ApplicationPermission]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost("list")]
    [ApplicationPermission]
    public async Task<IActionResult> Create([FromBody] ListCategoryQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
    
    [HttpPost("delete")]
    [ApplicationPermission]
    public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand query)
    {
        return Ok(await _mediator.Send(query));
    }
}