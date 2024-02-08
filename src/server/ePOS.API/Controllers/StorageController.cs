using ePOS.Application.Commands.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ePOS.API.Controllers;

[ApiController]
[Route("api/v1/storage")]
public class StorageController : ControllerBase
{
    private readonly IMediator _mediator;

    public StorageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost("upload-mutiple")]
    public async Task<IActionResult> Upload([FromForm] UploadFilesCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}