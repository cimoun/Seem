using MediatR;
using Seem.Application.Features.Tasks.DTOs;

namespace Seem.Application.Features.Tasks.Commands.MoveTask;

public record MoveTaskCommand : IRequest<TaskDto>
{
    public Guid Id { get; init; }
    public Guid? BoardColumnId { get; init; }
    public int SortOrder { get; init; }
}
