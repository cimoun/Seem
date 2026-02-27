using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Tasks.DTOs;

namespace Seem.Application.Features.Tasks.Queries.ListTasks;

public class ListTasksQueryHandler : IRequestHandler<ListTasksQuery, List<TaskDto>>
{
    private readonly IApplicationDbContext _context;

    public ListTasksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> Handle(ListTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _context.TaskItems
            .Include(t => t.Tags)
            .Include(t => t.Project)
            .Include(t => t.Subtasks)
            .Where(t => !t.IsDeleted)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            query = query.Where(t => t.ProjectId == request.ProjectId.Value);

        if (request.Status.HasValue)
            query = query.Where(t => t.Status == request.Status.Value);

        if (request.Priority.HasValue)
            query = query.Where(t => t.Priority == request.Priority.Value);

        var tasks = await query
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.SortOrder)
            .ThenByDescending(t => t.CreatedAt)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                TaskKey = t.TaskKey,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                ProjectId = t.ProjectId,
                ProjectName = t.Project.Name,
                BoardColumnId = t.BoardColumnId,
                ParentTaskId = t.ParentTaskId,
                DueDate = t.DueDate,
                StartDate = t.StartDate,
                CompletedAt = t.CompletedAt,
                EstimatedMinutes = t.EstimatedMinutes,
                ActualMinutes = t.ActualMinutes,
                SortOrder = t.SortOrder,
                IsOverdue = t.DueDate.HasValue && t.DueDate.Value < DateTime.UtcNow && t.Status != Domain.Enums.TaskItemStatus.Done && t.Status != Domain.Enums.TaskItemStatus.Cancelled,
                SubtaskCount = t.Subtasks.Count,
                CompletedSubtaskCount = t.Subtasks.Count(s => s.Status == Domain.Enums.TaskItemStatus.Done),
                Tags = t.Tags.Select(tag => new TagDto { Id = tag.Id, Name = tag.Name, Color = tag.Color.HexValue }).ToList(),
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return tasks;
    }
}
