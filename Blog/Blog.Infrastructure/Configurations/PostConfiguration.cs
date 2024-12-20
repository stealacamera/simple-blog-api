using Blog.Domain.Common.Enums;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

internal class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Content)
               .HasMaxLength(1000)
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.HasOne<PostStatuses>()
               .WithMany()
               .HasForeignKey(e => e.PostStatusId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(e => e.OwnerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
