using Seem.Domain.Enums;

namespace Seem.Application.Features.Projects.DTOs;

public record ProjectDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Key { get; init; } = null!;
    public string Color { get; init; } = null!;
    public bool IsArchived { get; init; }
    public int TaskCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record ProjectDetailDto : ProjectDto
{
    public List<BoardDto> Boards { get; init; } = new();
}

public record BoardDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public Guid ProjectId { get; init; }
    public List<BoardColumnDto> Columns { get; init; } = new();
}

public record BoardColumnDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public int Position { get; init; }
    public TaskItemStatus? MappedStatus { get; init; }
    public int? WipLimit { get; init; }
}
