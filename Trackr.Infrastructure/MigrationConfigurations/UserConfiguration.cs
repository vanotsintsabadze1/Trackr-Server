using Trackr.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trackr.Infrastructure.MigrationConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey("Id");
        builder.Property(u => u.Name).IsRequired().HasMaxLength(30);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Balance).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(u => u.CostLimit).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(u => u.EmailConfirmed).IsRequired().HasDefaultValue(false).HasColumnType("bit");
        builder.Property(u => u.CreatedAt).IsRequired().HasColumnType("datetime").HasDefaultValue(DateTime.UtcNow);
    }
}