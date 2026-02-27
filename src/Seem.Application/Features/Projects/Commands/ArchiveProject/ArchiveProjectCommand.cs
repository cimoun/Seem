using MediatR;

namespace Seem.Application.Features.Projects.Commands.ArchiveProject;

public record ArchiveProjectCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
}
