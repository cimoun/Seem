using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Tasks.Commands.MoveTask;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public MoveTaskCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<TaskDto> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.TaskItems
            .Include(t => t.Tags)
            .Include(t => t.Project)
            .Include(t => t.Subtasks)
            .FirstOrDefaultAsync(t => t.Id == request.Id && !t.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Task with ID '{request.Id}' not found.");

        task.MoveTo(request.BoardColumnId, request.SortOrder);

        // If the target column has a mapped status, update the task status accordingly
        if (request.BoardColumnId.HasValue)
        {
            var column = await _context.BoardColumns.FindAsync([request.BoardColumnId.Value], cancellationToken);
            if (column?.MappedStatus.HasValue == true)
            {
                task.ChangeStatus(column.MappedStatus.Value);
            }
        }

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
            ProjectName = task.Project.Name,
            BoardColumnId = task.BoardColumnId,
            ParentTaskId = task.ParentTaskId,
            DueDate = task.DueDate,
            StartDate = task.StartDate,
            CompletedAt = task.CompletedAt,
            EstimatedMinutes = task.EstimatedMinutes,
            ActualMinutes = task.ActualMinutes,
            SortOrder = task.SortOrder,
            IsOverdue = task.IsOverdue,
            CompletionPercentage = task.CompletionPercentage,
            SubtaskCount = task.Subtasks.Count,
            CompletedSubtaskCount = task.Subtasks.Count(s => s.Status == Domain.Enums.TaskItemStatus.Done),
            Tags = task.Tags.Select(t => new TagDto { Id = t.Id, Name = t.Name, Color = t.Color.HexValue }).ToList(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
