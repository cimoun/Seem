using Seem.Domain.Exceptions;

namespace Seem.Domain.ValueObjects;

public record TaskKey
{
    public string Value { get; }

    public TaskKey(string projectKey, int taskNumber)
    {
        if (string.IsNullOrWhiteSpace(projectKey))
            throw new DomainException("Project key is required.");
        if (taskNumber <= 0)
            throw new DomainException("Task number must be positive.");

        Value = $"{projectKey.ToUpperInvariant()}-{taskNumber}";
    }

    public static implicit operator string(TaskKey key) => key.Value;
    public override string ToString() => Value;
}
