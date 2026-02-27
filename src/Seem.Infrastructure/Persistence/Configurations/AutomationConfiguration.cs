using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seem.Domain.Entities.Automation;
using Seem.Domain.Entities.TaskManagement;

namespace Seem.Infrastructure.Persistence.Configurations;

public class AutomationRuleConfiguration : IEntityTypeConfiguration<AutomationRule>
{
    public void Configure(EntityTypeBuilder<AutomationRule> builder)
    {
        builder.ToTable("automation_rules");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.IsEnabled).HasDefaultValue(true);
        builder.Property(e => e.ExecutionCount).HasDefaultValue(0);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        builder.Property(e => e.TriggerConditions).HasColumnType("jsonb");

        builder.HasOne(e => e.Project)
            .WithMany()
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class RuleActionConfiguration : IEntityTypeConfiguration<Domain.Entities.Automation.RuleAction>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Automation.RuleAction> builder)
    {
        builder.ToTable("rule_actions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Parameters).HasColumnType("jsonb");

        builder.HasOne(e => e.AutomationRule)
            .WithMany(r => r.Actions)
            .HasForeignKey(e => e.AutomationRuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RecurringTaskConfiguration : IEntityTypeConfiguration<RecurringTask>
{
    public void Configure(EntityTypeBuilder<RecurringTask> builder)
    {
        builder.ToTable("recurring_tasks");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).HasMaxLength(500).IsRequired();
        builder.Property(e => e.CronExpression).HasMaxLength(100);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasOne(e => e.Project)
            .WithMany()
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.NextOccurrence);
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.ToTable("reminders");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Message).HasMaxLength(500).IsRequired();
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasOne(e => e.TaskItem)
            .WithMany()
            .HasForeignKey(e => e.TaskItemId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.RemindAt);
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.ToTable("task_comments");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Content).HasColumnType("text").IsRequired();

        builder.HasOne(e => e.TaskItem)
            .WithMany(t => t.Comments)
            .HasForeignKey(e => e.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TaskDependencyConfiguration : IEntityTypeConfiguration<TaskDependency>
{
    public void Configure(EntityTypeBuilder<TaskDependency> builder)
    {
        builder.ToTable("task_dependencies");
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.TaskItem)
            .WithMany(t => t.Dependencies)
            .HasForeignKey(e => e.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.DependsOnTaskItem)
            .WithMany()
            .HasForeignKey(e => e.DependsOnTaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.TaskItemId, e.DependsOnTaskItemId }).IsUnique();
    }
}
