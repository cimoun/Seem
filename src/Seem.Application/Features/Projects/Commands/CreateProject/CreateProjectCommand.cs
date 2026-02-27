using MediatR;
using Seem.Application.Features.Projects.DTOs;

namespace Seem.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand : IRequest<ProjectDetailDto>
{
    public string Name { get; init; } = null!;
    public string Key { get; init; } = null!;
    public string? Description { get; init; }
    public string? Color { get; init; }
}
