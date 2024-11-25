using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.MigrationConfigurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey("Id");
        builder.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.Title).IsRequired().HasMaxLength(50);
        builder.Property(t => t.Description).IsRequired().HasMaxLength(200);
        builder.HasOne<User>().WithMany().HasForeignKey(t => t.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}