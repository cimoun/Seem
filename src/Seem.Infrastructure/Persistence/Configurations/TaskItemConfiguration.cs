using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seem.Domain.Entities.TaskManagement;

namespace Seem.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("task_items");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TaskKey).HasMaxLength(20).IsRequired();
        builder.HasIndex(e => e.TaskKey).IsUnique();

        builder.Property(e => e.Title).HasMaxLength(500).IsRequired();
        builder.Property(e => e.Description).HasColumnType("text");
        builder.Property(e => e.Status).HasDefaultValue(Domain.Enums.TaskItemStatus.Todo);
        builder.Property(e => e.Priority).HasDefaultValue(Domain.Enums.Priority.Medium);
        builder.Property(e => e.SortOrder).HasDefaultValue(0);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.Property(e => e.Metadata)
            .HasColumnType("jsonb");

        // Relationships
        builder.HasOne(e => e.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.BoardColumn)
            .WithMany(c => c.Tasks)
            .HasForeignKey(e => e.BoardColumnId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.ParentTask)
            .WithMany(t => t.Subtasks)
            .HasForeignKey(e => e.ParentTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Tags)
            .WithMany(t => t.Tasks)
            .UsingEntity("task_tags");

        // Indexes
        builder.HasIndex(e => e.ProjectId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.DueDate);
        builder.HasIndex(e => e.BoardColumnId);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
