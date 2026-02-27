using Seem.Domain.Common;
using Seem.Domain.Enums;

namespace Seem.Domain.Entities.TaskManagement;

public class BoardColumn : BaseEntity
{
    public string Name { get; internal set; } = null!;
    public int Position { get; internal set; }
    public Guid BoardId { get; internal set; }
    public Board Board { get; private set; } = null!;
    public TaskItemStatus? MappedStatus { get; internal set; }
    public int? WipLimit { get; internal set; }

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    internal BoardColumn() { }

    public void Update(string name, TaskItemStatus? mappedStatus, int? wipLimit)
    {
        Name = name;
        MappedStatus = mappedStatus;
        WipLimit = wipLimit;
    }
}
