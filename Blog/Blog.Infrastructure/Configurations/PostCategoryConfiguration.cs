using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

internal class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.ToTable("PostCategories");

        builder.HasKey(e => new { e.PostId, e.CategoryId });

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.HasOne<Post>()
               .WithMany()
               .HasForeignKey(e => e.PostId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Category>()
               .WithMany()
               .HasForeignKey(e => e.CategoryId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
