using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class KargomatConfiguration : IEntityTypeConfiguration<Kargomat>
{
    public void Configure(EntityTypeBuilder<Kargomat> builder)
    {
        builder.Property(x => x.ShortAddress).IsRequired().HasMaxLength(256);
        builder.Property(x => x.FullAddress).IsRequired().HasMaxLength(512);

        builder.Property(x => x.Price).HasPrecision(10, 2);
    }
}
