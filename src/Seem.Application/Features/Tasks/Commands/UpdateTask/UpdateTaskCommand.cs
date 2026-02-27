using MediatR;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Enums;

namespace Seem.Application.Features.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand : IRequest<TaskDto>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public Priority Priority { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? StartDate { get; init; }
}
