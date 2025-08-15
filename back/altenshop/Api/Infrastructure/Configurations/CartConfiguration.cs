using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> b)
    {
        b.ToTable("Carts");
        b.HasKey(x => x.Id);

        b.HasIndex(x => x.UserId).IsUnique();

        b.HasMany(x => x.Items)
         .WithOne(i => i.Cart!)
         .HasForeignKey(i => i.CartId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}