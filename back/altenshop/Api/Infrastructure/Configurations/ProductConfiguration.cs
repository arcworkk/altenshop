using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Api.Domain.Entities;

namespace Api.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("Products");
        b.HasKey(x => x.Id);

        b.Property(x => x.Name).IsRequired().HasMaxLength(200);
        b.Property(x => x.Description).IsRequired();
        b.Property(x => x.Category).IsRequired().HasMaxLength(100);
        b.Property(x => x.Price).IsRequired().HasColumnType("decimal(10,2)");

        b.Property(x => x.Code).HasMaxLength(50);
        b.Property(x => x.InventoryStatus).HasMaxLength(30);
        b.Property(x => x.InternalReference).HasMaxLength(100);
    }
}