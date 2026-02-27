using Seem.Domain.Entities.KnowledgeBase;

namespace Seem.Domain.Interfaces;

public interface IArticleRepository : IRepository<Article>
{
    Task<IReadOnlyList<Article>> GetBySpaceAsync(Guid spaceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Article>> SearchAsync(string query, CancellationToken cancellationToken = default);
}
