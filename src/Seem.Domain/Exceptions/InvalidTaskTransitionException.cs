using Seem.Domain.Enums;

namespace Seem.Domain.Exceptions;

public class InvalidTaskTransitionException : DomainException
{
    public TaskItemStatus FromStatus { get; }
    public TaskItemStatus ToStatus { get; }

    public InvalidTaskTransitionException(TaskItemStatus from, TaskItemStatus to)
        : base($"Invalid task status transition from '{from}' to '{to}'.")
    {
        FromStatus = from;
        ToStatus = to;
    }
}
