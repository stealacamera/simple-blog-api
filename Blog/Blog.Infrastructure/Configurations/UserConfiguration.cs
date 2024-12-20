using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Username)
               .HasMaxLength(100)   
               .IsRequired();

        builder.Property(e => e.Email)
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(e => e.Password)
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.HasIndex(e => e.Username)
               .IsUnique();

        builder.HasIndex(e => e.Email)
               .IsUnique();
    }
}
