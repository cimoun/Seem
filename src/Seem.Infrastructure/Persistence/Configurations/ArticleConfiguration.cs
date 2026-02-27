using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seem.Domain.Entities.KnowledgeBase;

namespace Seem.Infrastructure.Persistence.Configurations;

public class SpaceConfiguration : IEntityTypeConfiguration<Space>
{
    public void Configure(EntityTypeBuilder<Space> builder)
    {
        builder.ToTable("spaces");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Icon).HasMaxLength(50);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).HasMaxLength(500).IsRequired();
        builder.Property(e => e.Content).HasColumnType("text");
        builder.Property(e => e.Slug).HasMaxLength(500);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasOne(e => e.Space)
            .WithMany(s => s.Articles)
            .HasForeignKey(e => e.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.ParentArticle)
            .WithMany(a => a.ChildArticles)
            .HasForeignKey(e => e.ParentArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Tags)
            .WithMany(t => t.Articles)
            .UsingEntity("article_tags");

        builder.HasIndex(e => e.SpaceId);
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class ArticleRevisionConfiguration : IEntityTypeConfiguration<ArticleRevision>
{
    public void Configure(EntityTypeBuilder<ArticleRevision> builder)
    {
        builder.ToTable("article_revisions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.PreviousContent).HasColumnType("text").IsRequired();
        builder.Property(e => e.ChangeNote).HasMaxLength(500);

        builder.HasOne(e => e.Article)
            .WithMany(a => a.Revisions)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
