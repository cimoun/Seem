using Seem.Domain.Common;

namespace Seem.Domain.Events;

public class TaskCompletedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public string TaskKey { get; }

    public TaskCompletedEvent(Guid taskId, string taskKey)
    {
        TaskId = taskId;
        TaskKey = taskKey;
    }
}
