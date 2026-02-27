using MediatR;
using Seem.Application.Features.Projects.DTOs;

namespace Seem.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand : IRequest<ProjectDto>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}
