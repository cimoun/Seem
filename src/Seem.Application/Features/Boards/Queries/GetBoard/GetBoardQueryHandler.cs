using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Boards.DTOs;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Boards.Queries.GetBoard;

public class GetBoardQueryHandler : IRequestHandler<GetBoardQuery, BoardWithTasksDto>
{
    private readonly IApplicationDbContext _context;

    public GetBoardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BoardWithTasksDto> Handle(GetBoardQuery request, CancellationToken cancellationToken)
    {
        var board = await _context.Boards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
            ?? throw new DomainException($"Board with ID '{request.Id}' not found.");

        var columnIds = board.Columns.Select(c => c.Id).ToList();

        var tasks = await _context.TaskItems
            .Include(t => t.Tags)
            .Include(t => t.Project)
            .Include(t => t.Subtasks)
            .Where(t => t.BoardColumnId.HasValue && columnIds.Contains(t.BoardColumnId.Value) && !t.IsDeleted)
            .ToListAsync(cancellationToken);

        var tasksByColumn = tasks
            .GroupBy(t => t.BoardColumnId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(t => t.SortOrder).ToList());

        return new BoardWithTasksDto
        {
            Id = board.Id,
            Name = board.Name,
            ProjectId = board.ProjectId,
            Columns = board.Columns
                .OrderBy(c => c.Position)
                .Select(c => new ColumnWithTasksDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Position = c.Position,
                    MappedStatus = c.MappedStatus,
                    WipLimit = c.WipLimit,
                    Tasks = tasksByColumn.TryGetValue(c.Id, out var columnTasks)
                        ? columnTasks.Select(t => new TaskDto
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
                            IsOverdue = t.IsOverdue,
                            CompletionPercentage = t.CompletionPercentage,
                            SubtaskCount = t.Subtasks.Count,
                            CompletedSubtaskCount = t.Subtasks.Count(s => s.Status == Domain.Enums.TaskItemStatus.Done),
                            Tags = t.Tags.Select(tag => new TagDto { Id = tag.Id, Name = tag.Name, Color = tag.Color.HexValue }).ToList(),
                            CreatedAt = t.CreatedAt,
                            UpdatedAt = t.UpdatedAt
                        }).ToList()
                        : new List<TaskDto>()
                }).ToList()
        };
    }
}
