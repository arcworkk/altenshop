using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> b)
    {
        b.ToTable("Wishlists");
        b.HasKey(x => x.Id);

        b.HasIndex(x => x.UserId).IsUnique();

        b.HasMany(x => x.Items)
         .WithOne(i => i.Wishlist!)
         .HasForeignKey(i => i.WishlistId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}