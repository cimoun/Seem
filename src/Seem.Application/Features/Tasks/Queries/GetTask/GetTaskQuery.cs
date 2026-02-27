using MediatR;
using Seem.Application.Features.Tasks.DTOs;

namespace Seem.Application.Features.Tasks.Queries.GetTask;

public record GetTaskQuery(Guid Id) : IRequest<TaskDto>;
