using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.Property(x => x.StoreName).HasMaxLength(128);
        builder.Property(x => x.TrackingNumber).HasMaxLength(128);
        builder.Property(x => x.DeclaredPrice).HasPrecision(10, 2);
        builder.Property(x => x.Weight).HasPrecision(10, 2);
        builder.Property(x => x.ShippingPrice).HasPrecision(10, 2);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.Packages)
            .HasForeignKey(x => x.OrderId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Customer)
         .WithMany(x => x.Packages)
         .HasForeignKey(x => x.CustomerId)
         .HasPrincipalKey(x => x.Id)
         .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DeliveryType)
         .WithMany(x => x.Packages)
         .HasForeignKey(x => x.DeliveryTypeId)
         .HasPrincipalKey(x => x.Id)
         .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ShipmentType)
            .WithMany(x => x.Packages)
            .HasForeignKey(x => x.ShipmentTypeId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Kargomat)
            .WithMany(x => x.Packages)
            .HasForeignKey(x => x.KargomatId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Country)
            .WithMany(x => x.Packages)
            .HasForeignKey(x => x.CountryId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TrackingNumber).IsUnique();
    }
}
