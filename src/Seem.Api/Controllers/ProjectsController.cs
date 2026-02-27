using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seem.Application.Features.Projects.Commands.ArchiveProject;
using Seem.Application.Features.Projects.Commands.CreateProject;
using Seem.Application.Features.Projects.Commands.UpdateProject;
using Seem.Application.Features.Projects.DTOs;
using Seem.Application.Features.Projects.Queries.GetProject;
using Seem.Application.Features.Projects.Queries.ListProjects;

namespace Seem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListProjectsQuery(), cancellationToken);
        return Ok(new { data = result });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectQuery(id), cancellationToken);
        return Ok(new { data = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, new { data = result });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };
        var result = await _mediator.Send(commandWithId, cancellationToken);
        return Ok(new { data = result });
    }

    [HttpPatch("{id:guid}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Archive(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ArchiveProjectCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}
