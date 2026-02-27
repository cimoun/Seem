using Seem.Domain.Common;
using Seem.Domain.Exceptions;
using Seem.Domain.ValueObjects;

namespace Seem.Domain.Entities.TaskManagement;

public class Project : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Key { get; private set; } = null!;
    public Color Color { get; private set; } = new("#3B82F6");
    public bool IsArchived { get; private set; }
    public int NextTaskNumber { get; private set; } = 1;

    private readonly List<Board> _boards = new();
    public IReadOnlyCollection<Board> Boards => _boards.AsReadOnly();

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    private Project() { }

    public static Project Create(string name, string key, string? description = null, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Project name is required.");
        if (string.IsNullOrWhiteSpace(key) || key.Length > 5)
            throw new DomainException("Project key must be 1-5 characters.");

        return new Project
        {
            Name = name,
            Key = key.ToUpperInvariant(),
            Description = description,
            Color = color != null ? new Color(color) : new Color("#3B82F6")
        };
    }

    public TaskKey GenerateTaskKey()
    {
        var taskKey = new TaskKey(Key, NextTaskNumber);
        NextTaskNumber++;
        return taskKey;
    }

    public Board AddBoard(string name)
    {
        var board = Board.Create(name, Id);
        _boards.Add(board);
        return board;
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Project name is required.");
        Name = name;
        Description = description;
    }

    public void Archive() => IsArchived = true;
    public void Unarchive() => IsArchived = false;
}
