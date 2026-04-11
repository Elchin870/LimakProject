using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(256);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(256);
        builder.HasOne(x => x.Customer).WithOne(x => x.AppUser).HasForeignKey<Customer>(x => x.AppUserId);
    }
}
