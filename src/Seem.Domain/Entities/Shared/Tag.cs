using Seem.Domain.Common;
using Seem.Domain.Entities.KnowledgeBase;
using Seem.Domain.Entities.TaskManagement;
using Seem.Domain.ValueObjects;

namespace Seem.Domain.Entities.Shared;

public class Tag : BaseEntity
{
    public string Name { get; private set; } = null!;
    public Color Color { get; private set; } = new("#6B7280");

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    private readonly List<Article> _articles = new();
    public IReadOnlyCollection<Article> Articles => _articles.AsReadOnly();

    private Tag() { }

    public static Tag Create(string name, string? color = null)
    {
        return new Tag
        {
            Name = name,
            Color = color != null ? new Color(color) : new Color("#6B7280")
        };
    }

    public void Update(string name, string? color)
    {
        Name = name;
        if (color != null) Color = new Color(color);
    }
}
