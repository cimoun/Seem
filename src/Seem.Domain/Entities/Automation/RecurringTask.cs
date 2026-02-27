using Seem.Domain.Common;
using Seem.Domain.Entities.TaskManagement;
using Seem.Domain.Enums;

namespace Seem.Domain.Entities.Automation;

public class RecurringTask : AuditableEntity
{
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public Guid? BoardColumnId { get; private set; }
    public Priority Priority { get; private set; }

    public RecurrencePattern Pattern { get; private set; }
    public string? CronExpression { get; private set; }
    public DateTime NextOccurrence { get; private set; }
    public DateTime? LastGenerated { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int? DueDateOffsetDays { get; private set; }

    private RecurringTask() { }

    public static RecurringTask Create(
        string title, Guid projectId, RecurrencePattern pattern,
        DateTime nextOccurrence, Priority priority = Priority.Medium,
        string? cronExpression = null, int? dueDateOffsetDays = null)
    {
        return new RecurringTask
        {
            Title = title,
            ProjectId = projectId,
            Pattern = pattern,
            NextOccurrence = nextOccurrence,
            Priority = priority,
            CronExpression = cronExpression,
            DueDateOffsetDays = dueDateOffsetDays
        };
    }

    public void Toggle() => IsActive = !IsActive;

    public void RecordGeneration(DateTime nextOccurrence)
    {
        LastGenerated = DateTime.UtcNow;
        NextOccurrence = nextOccurrence;
    }
}
