using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class DeliveryTypeConfiguration : IEntityTypeConfiguration<DeliveryType>
{
    public void Configure(EntityTypeBuilder<DeliveryType> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }
}
