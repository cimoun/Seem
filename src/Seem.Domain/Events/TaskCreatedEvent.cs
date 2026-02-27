using Seem.Domain.Common;

namespace Seem.Domain.Events;

public class TaskCreatedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public string TaskKey { get; }

    public TaskCreatedEvent(Guid taskId, string taskKey)
    {
        TaskId = taskId;
        TaskKey = taskKey;
    }
}
