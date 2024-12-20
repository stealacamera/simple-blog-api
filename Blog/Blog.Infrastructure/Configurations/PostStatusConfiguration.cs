using Blog.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

internal class PostStatusConfiguration : IEntityTypeConfiguration<PostStatuses>
{
    public void Configure(EntityTypeBuilder<PostStatuses> builder)
    {
        builder.ToTable("PostStatuses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(30)   
               .IsRequired();

        builder.Property(e => e.Value)
               .IsRequired();

        builder.HasData(PostStatuses.List);
    }
}
