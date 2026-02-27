using Seem.Domain.Common;
using Seem.Domain.Entities.TaskManagement;

namespace Seem.Domain.Entities.Automation;

public class Reminder : AuditableEntity
{
    public string Message { get; private set; } = null!;
    public DateTime RemindAt { get; private set; }
    public bool IsDismissed { get; private set; }
    public bool IsTriggered { get; private set; }

    public Guid? TaskItemId { get; private set; }
    public TaskItem? TaskItem { get; private set; }

    private Reminder() { }

    public static Reminder Create(string message, DateTime remindAt, Guid? taskItemId = null)
    {
        return new Reminder
        {
            Message = message,
            RemindAt = remindAt,
            TaskItemId = taskItemId
        };
    }

    public void Dismiss() => IsDismissed = true;
    public void MarkTriggered() => IsTriggered = true;
}
