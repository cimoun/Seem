using Seem.Domain.Common;
using Seem.Domain.Entities.Shared;

namespace Seem.Domain.Entities.KnowledgeBase;

public class Article : AuditableEntity
{
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = string.Empty;
    public string? Slug { get; private set; }
    public bool IsPinned { get; private set; }

    public Guid SpaceId { get; private set; }
    public Space Space { get; private set; } = null!;

    public Guid? ParentArticleId { get; private set; }
    public Article? ParentArticle { get; private set; }
    public int SortOrder { get; private set; }
    public int Depth { get; private set; }

    private readonly List<Article> _childArticles = new();
    public IReadOnlyCollection<Article> ChildArticles => _childArticles.AsReadOnly();

    private readonly List<ArticleRevision> _revisions = new();
    public IReadOnlyCollection<ArticleRevision> Revisions => _revisions.AsReadOnly();

    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

    private Article() { }

    public static Article Create(string title, Guid spaceId, Guid? parentArticleId = null, int depth = 0)
    {
        return new Article
        {
            Title = title,
            SpaceId = spaceId,
            ParentArticleId = parentArticleId,
            Depth = depth
        };
    }

    public ArticleRevision UpdateContent(string newContent, string? changeNote = null)
    {
        var revision = ArticleRevision.Create(Id, Content, changeNote);
        _revisions.Add(revision);
        Content = newContent;
        return revision;
    }

    public void Update(string title, string? slug)
    {
        Title = title;
        Slug = slug;
    }

    public void TogglePin() => IsPinned = !IsPinned;
}
