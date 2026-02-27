using Microsoft.EntityFrameworkCore;
using Seem.Domain.Entities.Automation;
using Seem.Domain.Entities.KnowledgeBase;
using Seem.Domain.Entities.Shared;
using Seem.Domain.Entities.TaskManagement;

namespace Seem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Project> Projects { get; }
    DbSet<Board> Boards { get; }
    DbSet<BoardColumn> BoardColumns { get; }
    DbSet<TaskItem> TaskItems { get; }
    DbSet<TaskComment> TaskComments { get; }
    DbSet<TaskDependency> TaskDependencies { get; }
    DbSet<Tag> Tags { get; }
    DbSet<User> Users { get; }
    DbSet<Space> Spaces { get; }
    DbSet<Article> Articles { get; }
    DbSet<ArticleRevision> ArticleRevisions { get; }
    DbSet<AutomationRule> AutomationRules { get; }
    DbSet<RuleAction> RuleActions { get; }
    DbSet<RecurringTask> RecurringTasks { get; }
    DbSet<Reminder> Reminders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
