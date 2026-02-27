using Seem.Domain.Common;

namespace Seem.Domain.Entities.KnowledgeBase;

public class ArticleRevision : BaseEntity
{
    public Guid ArticleId { get; private set; }
    public Article Article { get; private set; } = null!;
    public string PreviousContent { get; private set; } = null!;
    public string? ChangeNote { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ArticleRevision() { }

    internal static ArticleRevision Create(Guid articleId, string previousContent, string? changeNote)
    {
        return new ArticleRevision
        {
            ArticleId = articleId,
            PreviousContent = previousContent,
            ChangeNote = changeNote,
            CreatedAt = DateTime.UtcNow
        };
    }
}
