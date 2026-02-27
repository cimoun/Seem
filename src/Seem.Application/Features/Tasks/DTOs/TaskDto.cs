using Seem.Domain.Enums;

namespace Seem.Application.Features.Tasks.DTOs;

public record TaskDto
{
    public Guid Id { get; init; }
    public string TaskKey { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public TaskItemStatus Status { get; init; }
    public Priority Priority { get; init; }
    public Guid ProjectId { get; init; }
    public string? ProjectName { get; init; }
    public Guid? BoardColumnId { get; init; }
    public Guid? ParentTaskId { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? CompletedAt { get; init; }
    public int? EstimatedMinutes { get; init; }
    public int? ActualMinutes { get; init; }
    public int SortOrder { get; init; }
    public bool IsOverdue { get; init; }
    public double? CompletionPercentage { get; init; }
    public int SubtaskCount { get; init; }
    public int CompletedSubtaskCount { get; init; }
    public List<TagDto> Tags { get; init; } = new();
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record TagDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
}
