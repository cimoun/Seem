using Seem.Domain.Common;
using Seem.Domain.Entities.Shared;
using Seem.Domain.Enums;
using Seem.Domain.Events;
using Seem.Domain.Exceptions;

namespace Seem.Domain.Entities.TaskManagement;

public class TaskItem : AuditableEntity
{
    public string TaskKey { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public TaskItemStatus Status { get; private set; } = TaskItemStatus.Todo;
    public Priority Priority { get; private set; } = Priority.Medium;

    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    public Guid? BoardColumnId { get; private set; }
    public BoardColumn? BoardColumn { get; private set; }

    public Guid? ParentTaskId { get; private set; }
    public TaskItem? ParentTask { get; private set; }

    public DateTime? DueDate { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public int? EstimatedMinutes { get; private set; }
    public int? ActualMinutes { get; private set; }
    public int SortOrder { get; private set; }

    public Dictionary<string, object>? Metadata { get; private set; }

    private readonly List<TaskItem> _subtasks = new();
    public IReadOnlyCollection<TaskItem> Subtasks => _subtasks.AsReadOnly();

    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

    private readonly List<TaskComment> _comments = new();
    public IReadOnlyCollection<TaskComment> Comments => _comments.AsReadOnly();

    private readonly List<TaskDependency> _dependencies = new();
    public IReadOnlyCollection<TaskDependency> Dependencies => _dependencies.AsReadOnly();

    private TaskItem() { }

    public static TaskItem Create(
        string title,
        string taskKey,
        Guid projectId,
        Priority priority = Priority.Medium,
        DateTime? dueDate = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task title is required.");

        var task = new TaskItem
        {
            Title = title,
            TaskKey = taskKey,
            ProjectId = projectId,
            Priority = priority,
            DueDate = dueDate,
            Description = description
        };

        task.AddDomainEvent(new TaskCreatedEvent(task.Id, task.TaskKey));
        return task;
    }

    public void ChangeStatus(TaskItemStatus newStatus)
    {
        ValidateTransition(Status, newStatus);
        var oldStatus = Status;
        Status = newStatus;

        if (newStatus == TaskItemStatus.Done)
        {
            CompletedAt = DateTime.UtcNow;
            AddDomainEvent(new TaskCompletedEvent(Id, TaskKey));
        }

        AddDomainEvent(new TaskStatusChangedEvent(Id, oldStatus, newStatus));
    }

    public void SetDeadline(DateTime? dueDate)
    {
        DueDate = dueDate;
    }

    public void SetDates(DateTime? startDate, DateTime? dueDate)
    {
        StartDate = startDate;
        DueDate = dueDate;
    }

    public void Update(string title, string? description, Priority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task title is required.");
        Title = title;
        Description = description;
        Priority = priority;
    }

    public void SetTimeTracking(int? estimatedMinutes, int? actualMinutes)
    {
        EstimatedMinutes = estimatedMinutes;
        ActualMinutes = actualMinutes;
    }

    public void MoveTo(Guid? boardColumnId, int sortOrder)
    {
        BoardColumnId = boardColumnId;
        SortOrder = sortOrder;
    }

    public TaskItem AddSubtask(string title, string taskKey)
    {
        var subtask = new TaskItem
        {
            Title = title,
            TaskKey = taskKey,
            ProjectId = ProjectId,
            ParentTaskId = Id,
            SortOrder = _subtasks.Count
        };
        _subtasks.Add(subtask);
        return subtask;
    }

    public void AddTag(Tag tag)
    {
        if (!_tags.Contains(tag))
            _tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        _tags.Remove(tag);
    }

    public TaskComment AddComment(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Comment content is required.");

        var comment = TaskComment.Create(content, Id);
        _comments.Add(comment);
        return comment;
    }

    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != TaskItemStatus.Done && Status != TaskItemStatus.Cancelled;

    public double? CompletionPercentage =>
        _subtasks.Count == 0 ? null :
        (double)_subtasks.Count(s => s.Status == TaskItemStatus.Done) / _subtasks.Count * 100;

    private static void ValidateTransition(TaskItemStatus from, TaskItemStatus to)
    {
        if (from == to) return;

        var validTransitions = new Dictionary<TaskItemStatus, TaskItemStatus[]>
        {
            [TaskItemStatus.Todo] = [TaskItemStatus.InProgress, TaskItemStatus.Cancelled],
            [TaskItemStatus.InProgress] = [TaskItemStatus.InReview, TaskItemStatus.Blocked, TaskItemStatus.Done, TaskItemStatus.Cancelled],
            [TaskItemStatus.InReview] = [TaskItemStatus.InProgress, TaskItemStatus.Done],
            [TaskItemStatus.Blocked] = [TaskItemStatus.InProgress, TaskItemStatus.Cancelled],
            [TaskItemStatus.Done] = [TaskItemStatus.Todo],
            [TaskItemStatus.Cancelled] = [TaskItemStatus.Todo]
        };

        if (!validTransitions.TryGetValue(from, out var allowed) || !allowed.Contains(to))
            throw new InvalidTaskTransitionException(from, to);
    }
}
