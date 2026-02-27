using MediatR;

namespace Seem.Application.Features.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
}
