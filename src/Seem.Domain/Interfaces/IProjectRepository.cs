using Seem.Domain.Entities.TaskManagement;

namespace Seem.Domain.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<Project?> GetWithBoardsAsync(Guid id, CancellationToken cancellationToken = default);
}
