using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasOne(x => x.Office).WithMany(x => x.Customers).HasForeignKey(x => x.OfficeId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.AppUserId).IsUnique();

        builder.Property(x => x.CustomerCode).IsRequired().HasMaxLength(8);
        builder.HasIndex(x => x.CustomerCode).IsUnique();

        builder.Property(x => x.Balance).HasPrecision(10, 2);
    }
}
