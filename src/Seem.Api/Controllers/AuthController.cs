using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Auth.Commands.Login;
using Seem.Application.Features.Auth.Commands.Register;
using Seem.Application.Features.Auth.DTOs;

namespace Seem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, new { data = result });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new { data = result });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me([FromServices] ICurrentUser currentUser)
    {
        return Ok(new
        {
            data = new
            {
                userId = currentUser.UserId,
                username = currentUser.Username
            }
        });
    }
}
