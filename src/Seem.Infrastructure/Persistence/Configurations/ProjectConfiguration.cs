using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seem.Domain.Entities.TaskManagement;

namespace Seem.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasColumnType("text");
        builder.Property(e => e.Key).HasMaxLength(5).IsRequired();
        builder.HasIndex(e => e.Key).IsUnique();

        builder.OwnsOne(e => e.Color, color =>
        {
            color.Property(c => c.HexValue).HasColumnName("color").HasMaxLength(7);
        });

        builder.Property(e => e.NextTaskNumber).HasDefaultValue(1);
        builder.Property(e => e.IsArchived).HasDefaultValue(false);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
