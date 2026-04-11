using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class TariffConfiguration : IEntityTypeConfiguration<Tariff>
{
    public void Configure(EntityTypeBuilder<Tariff> builder)
    {
        builder.Property(x => x.MaxWeight).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.MinWeight).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.BasePrice).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.ExtraPricePerKg).HasPrecision(10, 2);

        builder.Property(x => x.BaseWeightLimit).IsRequired().HasPrecision(10, 2);
        builder.HasOne(x => x.DeliveryType).WithMany(x => x.Tariffs).HasForeignKey(x => x.DeliveryTypeId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Country).WithMany(x => x.Tariffs).HasForeignKey(x => x.CountryId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ShipmentType).WithMany(x => x.Tariffs).HasForeignKey(x => x.ShipmentTypeId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Restrict);
    }
}
