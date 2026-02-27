using Seem.Domain.Common;
using Seem.Domain.Entities.TaskManagement;
using Seem.Domain.Enums;

namespace Seem.Domain.Entities.Automation;

public class AutomationRule : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsEnabled { get; private set; } = true;
    public Guid? ProjectId { get; private set; }
    public Project? Project { get; private set; }

    public TriggerType TriggerType { get; private set; }
    public Dictionary<string, string>? TriggerConditions { get; private set; }

    private readonly List<RuleAction> _actions = new();
    public IReadOnlyCollection<RuleAction> Actions => _actions.AsReadOnly();

    public int ExecutionCount { get; private set; }
    public DateTime? LastExecutedAt { get; private set; }

    private AutomationRule() { }

    public static AutomationRule Create(string name, TriggerType triggerType, Guid? projectId = null, string? description = null)
    {
        return new AutomationRule
        {
            Name = name,
            TriggerType = triggerType,
            ProjectId = projectId,
            Description = description
        };
    }

    public RuleAction AddAction(ActionType type, int order, Dictionary<string, string>? parameters = null)
    {
        var action = RuleAction.Create(type, order, Id, parameters);
        _actions.Add(action);
        return action;
    }

    public void Toggle() => IsEnabled = !IsEnabled;

    public void RecordExecution()
    {
        ExecutionCount++;
        LastExecutedAt = DateTime.UtcNow;
    }
}
