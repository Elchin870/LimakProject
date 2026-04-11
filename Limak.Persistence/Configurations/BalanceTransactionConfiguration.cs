using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class BalanceTransactionConfiguration : IEntityTypeConfiguration<BalanceTransaction>
{
    public void Configure(EntityTypeBuilder<BalanceTransaction> builder)
    {
        builder.HasOne(bt => bt.Customer)
            .WithMany(c => c.BalanceTransactions)
            .HasForeignKey(bt => bt.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bt => bt.Order)
            .WithMany(o => o.BalanceTransactions)
            .HasForeignKey(bt => bt.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.Description).HasMaxLength(256);
        builder.Property(x => x.Amount).HasPrecision(10, 2);
    }
}
