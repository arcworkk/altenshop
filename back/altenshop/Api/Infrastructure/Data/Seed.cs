using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Infrastructure.Data;

public static class Seed
{
    public static async Task RunAsync(AppDbContext db)
    {
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new User { Username = "admin", Firstname= "adminFirstname", Email= "admin@admin.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = UserRole.Admin },
                new User { Username = "user", Firstname = "userFirstname", Email = "user@user.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"), Role = UserRole.User }
            );
        }

        if (!await db.Products.AnyAsync())
        {
            db.Products.AddRange(
                new Product
                {
                    Code = "P-001",
                    Name = "Produit A",
                    Description = "Desc A",
                    Image = "",
                    Category = "Cat 1",
                    Price = 10,
                    Quantity = 5,
                    InternalReference = "INT-1",
                    ShellId = 1,
                    InventoryStatus = "INSTOCK",
                    Rating = 4
                },
                new Product
                {
                    Code = "P-002",
                    Name = "Produit B",
                    Description = "Desc B",
                    Image = "",
                    Category = "Cat 2",
                    Price = 20,
                    Quantity = 2,
                    InternalReference = "INT-2",
                    ShellId = 2,
                    InventoryStatus = "LOWSTOCK",
                    Rating = 5
                }
            );
        }

        await db.SaveChangesAsync();
    }
}
