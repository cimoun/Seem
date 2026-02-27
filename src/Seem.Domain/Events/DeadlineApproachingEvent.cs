using Seem.Domain.Common;

namespace Seem.Domain.Events;

public class DeadlineApproachingEvent : DomainEvent
{
    public Guid TaskId { get; }
    public string TaskKey { get; }
    public DateTime DueDate { get; }
    public int DaysRemaining { get; }

    public DeadlineApproachingEvent(Guid taskId, string taskKey, DateTime dueDate, int daysRemaining)
    {
        TaskId = taskId;
        TaskKey = taskKey;
        DueDate = dueDate;
        DaysRemaining = daysRemaining;
    }
}
