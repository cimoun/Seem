using Seem.Domain.Common;

namespace Seem.Domain.Entities.KnowledgeBase;

public class Space : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? Icon { get; private set; }
    public int SortOrder { get; private set; }

    private readonly List<Article> _articles = new();
    public IReadOnlyCollection<Article> Articles => _articles.AsReadOnly();

    private Space() { }

    public static Space Create(string name, string? description = null, string? icon = null)
    {
        return new Space
        {
            Name = name,
            Description = description,
            Icon = icon
        };
    }

    public void Update(string name, string? description, string? icon)
    {
        Name = name;
        Description = description;
        Icon = icon;
    }
}
