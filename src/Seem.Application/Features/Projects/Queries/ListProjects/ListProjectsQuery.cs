using MediatR;
using Seem.Application.Features.Projects.DTOs;

namespace Seem.Application.Features.Projects.Queries.ListProjects;

public record ListProjectsQuery : IRequest<List<ProjectDto>>;
