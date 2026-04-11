using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class CountryAddressConfiguration : IEntityTypeConfiguration<CountryAddress>
{
    public void Configure(EntityTypeBuilder<CountryAddress> builder)
    {
        builder.Property(x => x.City).IsRequired().HasMaxLength(100);
        builder.Property(x => x.State).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(256);
        builder.Property(x => x.TC).HasMaxLength(100);
        builder.HasOne(x => x.Country).WithMany(x => x.CountryAddresses).HasForeignKey(x => x.CountryId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);
    }
}
