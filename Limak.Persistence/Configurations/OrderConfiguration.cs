using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.StoreName).IsRequired().HasMaxLength(128);
        builder.Property(x => x.StoreUrl).IsRequired().HasMaxLength(512);
        builder.Property(x => x.PriceAzn).HasPrecision(10, 2);
        builder.Property(x => x.Quantity).IsRequired();
        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(x => x.CustomerId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Country)
            .WithMany(c => c.Orders)
            .HasForeignKey(x => x.CountryId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.OrderNumber).IsUnique();
    }

}
