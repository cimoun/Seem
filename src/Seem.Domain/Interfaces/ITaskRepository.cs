using Seem.Domain.Entities.TaskManagement;
using Seem.Domain.Enums;

namespace Seem.Domain.Interfaces;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<IReadOnlyList<TaskItem>> GetByBoardColumnAsync(Guid boardColumnId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaskItem>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaskItem>> GetOverdueAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaskItem>> SearchAsync(string query, TaskItemStatus? status = null, Priority? priority = null, CancellationToken cancellationToken = default);
}
