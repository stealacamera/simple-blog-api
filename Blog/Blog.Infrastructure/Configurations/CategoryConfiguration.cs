using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(100)   
               .IsRequired();

        builder.Property(e => e.Description)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.HasIndex(e => e.Name)
               .IsUnique();
    }
}
