using Seem.Domain.Common;
using Seem.Domain.Exceptions;

namespace Seem.Domain.Entities.TaskManagement;

public class TaskComment : BaseEntity
{
    public string Content { get; private set; } = null!;
    public Guid TaskItemId { get; private set; }
    public TaskItem TaskItem { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TaskComment() { }

    internal static TaskComment Create(string content, Guid taskItemId)
    {
        return new TaskComment
        {
            Content = content,
            TaskItemId = taskItemId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Comment content is required.");
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }
}
