namespace Seem.Domain.Enums;

public enum TriggerType
{
    OnTaskCreated = 0,
    OnStatusChanged = 1,
    OnDueDateApproaching = 2,
    OnTaskOverdue = 3,
    OnSchedule = 4,
    OnTagAdded = 5
}
