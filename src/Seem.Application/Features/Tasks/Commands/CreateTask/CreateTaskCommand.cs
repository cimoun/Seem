using MediatR;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Enums;

namespace Seem.Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand : IRequest<TaskDto>
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public Guid ProjectId { get; init; }
    public Priority Priority { get; init; } = Priority.Medium;
    public DateTime? DueDate { get; init; }
    public Guid? BoardColumnId { get; init; }
    public Guid? ParentTaskId { get; init; }
    public List<Guid>? TagIds { get; init; }
}
