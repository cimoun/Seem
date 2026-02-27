using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seem.Application.Features.Tasks.Commands.ChangeTaskStatus;
using Seem.Application.Features.Tasks.Commands.CreateTask;
using Seem.Application.Features.Tasks.Commands.DeleteTask;
using Seem.Application.Features.Tasks.Commands.MoveTask;
using Seem.Application.Features.Tasks.Commands.UpdateTask;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Application.Features.Tasks.Queries.GetTask;
using Seem.Application.Features.Tasks.Queries.ListTasks;
using Seem.Domain.Enums;

namespace Seem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, new { data = result });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaskQuery(id), cancellationToken);
        return Ok(new { data = result });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TaskDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? projectId,
        [FromQuery] TaskItemStatus? status,
        [FromQuery] Priority? priority,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListTasksQuery
        {
            ProjectId = projectId,
            Status = status,
            Priority = priority
        }, cancellationToken);
        return Ok(new { data = result });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };
        var result = await _mediator.Send(commandWithId, cancellationToken);
        return Ok(new { data = result });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTaskCommand { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeTaskStatusCommand command, CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };
        var result = await _mediator.Send(commandWithId, cancellationToken);
        return Ok(new { data = result });
    }

    [HttpPatch("{id:guid}/move")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Move(Guid id, [FromBody] MoveTaskCommand command, CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };
        var result = await _mediator.Send(commandWithId, cancellationToken);
        return Ok(new { data = result });
    }
}
