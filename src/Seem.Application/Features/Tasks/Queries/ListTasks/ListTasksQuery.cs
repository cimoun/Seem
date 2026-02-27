using MediatR;
using Seem.Application.Features.Tasks.DTOs;
using Seem.Domain.Enums;

namespace Seem.Application.Features.Tasks.Queries.ListTasks;

public record ListTasksQuery : IRequest<List<TaskDto>>
{
    public Guid? ProjectId { get; init; }
    public TaskItemStatus? Status { get; init; }
    public Priority? Priority { get; init; }
}
