using Seem.Domain.Common;
using Seem.Domain.Enums;

namespace Seem.Domain.Events;

public class TaskStatusChangedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public TaskItemStatus OldStatus { get; }
    public TaskItemStatus NewStatus { get; }

    public TaskStatusChangedEvent(Guid taskId, TaskItemStatus oldStatus, TaskItemStatus newStatus)
    {
        TaskId = taskId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}
