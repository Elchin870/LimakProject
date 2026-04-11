using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class ShipmentTypeConfiguration : IEntityTypeConfiguration<ShipmentType>
{
    public void Configure(EntityTypeBuilder<ShipmentType> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }
}
