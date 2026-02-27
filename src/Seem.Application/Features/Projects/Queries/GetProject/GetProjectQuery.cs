using MediatR;
using Seem.Application.Features.Projects.DTOs;

namespace Seem.Application.Features.Projects.Queries.GetProject;

public record GetProjectQuery(Guid Id) : IRequest<ProjectDetailDto>;
