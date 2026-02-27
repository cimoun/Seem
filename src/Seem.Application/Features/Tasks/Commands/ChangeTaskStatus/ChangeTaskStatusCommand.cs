using MediatR;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Enums;

namespace Seem.Application.Features.Tasks.Commands.ChangeTaskStatus;

public record ChangeTaskStatusCommand : IRequest<TaskDto>
{
    public Guid Id { get; init; }
    public TaskItemStatus NewStatus { get; init; }
}
