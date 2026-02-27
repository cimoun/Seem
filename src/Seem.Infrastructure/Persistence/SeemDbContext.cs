using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Domain.Entities.Automation;
using Seem.Domain.Entities.KnowledgeBase;
using Seem.Domain.Entities.Shared;
using Seem.Domain.Entities.TaskManagement;
using Seem.Domain.Interfaces;

namespace Seem.Infrastructure.Persistence;

public class SeemDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    public SeemDbContext(DbContextOptions<SeemDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<BoardColumn> BoardColumns => Set<BoardColumn>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<TaskComment> TaskComments => Set<TaskComment>();
    public DbSet<TaskDependency> TaskDependencies => Set<TaskDependency>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Space> Spaces => Set<Space>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleRevision> ArticleRevisions => Set<ArticleRevision>();
    public DbSet<AutomationRule> AutomationRules => Set<AutomationRule>();
    public DbSet<RuleAction> RuleActions => Set<RuleAction>();
    public DbSet<RecurringTask> RecurringTasks => Set<RecurringTask>();
    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SeemDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
