using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Entities.TaskManagement;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public CreateTaskCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync([request.ProjectId], cancellationToken)
            ?? throw new DomainException($"Project with ID '{request.ProjectId}' not found.");

        var taskKey = project.GenerateTaskKey();

        var task = TaskItem.Create(
            request.Title,
            taskKey.Value,
            request.ProjectId,
            request.Priority,
            request.DueDate,
            request.Description
        );

        if (request.BoardColumnId.HasValue)
            task.MoveTo(request.BoardColumnId, 0);

        if (request.TagIds is { Count: > 0 })
        {
            var tags = await _context.Tags
                .Where(t => request.TagIds.Contains(t.Id))
                .ToListAsync(cancellationToken);

            foreach (var tag in tags)
                task.AddTag(tag);
        }

        await _context.TaskItems.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Dispatch domain events
        foreach (var domainEvent in task.DomainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
        task.ClearDomainEvents();

        return new TaskDto
        {
            Id = task.Id,
            TaskKey = task.TaskKey,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            ProjectId = task.ProjectId,
            ProjectName = project.Name,
            BoardColumnId = task.BoardColumnId,
            DueDate = task.DueDate,
            StartDate = task.StartDate,
            SortOrder = task.SortOrder,
            IsOverdue = task.IsOverdue,
            CreatedAt = task.CreatedAt,
            Tags = task.Tags.Select(t => new TagDto { Id = t.Id, Name = t.Name, Color = t.Color.HexValue }).ToList()
        };
    }
}
