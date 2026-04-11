using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limak.Persistence.Configurations;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(512);
        builder.Property(x => x.WebsitePath).IsRequired().HasMaxLength(512);

        builder.HasOne(x => x.Category).WithMany(x => x.Shops).HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);
    }
}
