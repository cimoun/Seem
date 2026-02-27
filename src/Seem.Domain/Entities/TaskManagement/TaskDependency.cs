using Seem.Domain.Common;
using Seem.Domain.Enums;

namespace Seem.Domain.Entities.TaskManagement;

public class TaskDependency : BaseEntity
{
    public Guid TaskItemId { get; private set; }
    public TaskItem TaskItem { get; private set; } = null!;

    public Guid DependsOnTaskItemId { get; private set; }
    public TaskItem DependsOnTaskItem { get; private set; } = null!;

    public DependencyType Type { get; private set; } = DependencyType.FinishToStart;

    private TaskDependency() { }

    public static TaskDependency Create(Guid taskItemId, Guid dependsOnTaskItemId, DependencyType type = DependencyType.FinishToStart)
    {
        return new TaskDependency
        {
            TaskItemId = taskItemId,
            DependsOnTaskItemId = dependsOnTaskItemId,
            Type = type
        };
    }
}
