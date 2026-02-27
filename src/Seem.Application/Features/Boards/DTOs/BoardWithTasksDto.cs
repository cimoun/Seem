using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Enums;

namespace Seem.Application.Features.Boards.DTOs;

public record BoardWithTasksDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public Guid ProjectId { get; init; }
    public List<ColumnWithTasksDto> Columns { get; init; } = new();
}

public record ColumnWithTasksDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public int Position { get; init; }
    public TaskItemStatus? MappedStatus { get; init; }
    public int? WipLimit { get; init; }
    public List<TaskDto> Tasks { get; init; } = new();
}
