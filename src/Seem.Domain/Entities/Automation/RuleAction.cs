using Seem.Domain.Common;
using Seem.Domain.Enums;

namespace Seem.Domain.Entities.Automation;

public class RuleAction : BaseEntity
{
    public ActionType Type { get; private set; }
    public int Order { get; private set; }
    public Dictionary<string, string>? Parameters { get; private set; }
    public Guid AutomationRuleId { get; private set; }
    public AutomationRule AutomationRule { get; private set; } = null!;

    private RuleAction() { }

    internal static RuleAction Create(ActionType type, int order, Guid automationRuleId, Dictionary<string, string>? parameters = null)
    {
        return new RuleAction
        {
            Type = type,
            Order = order,
            AutomationRuleId = automationRuleId,
            Parameters = parameters
        };
    }
}
