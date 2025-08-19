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
            var random = new Random();
            var categories = new[] { "Accessories", "Fitness", "Clothing", "Electronics" };
            var inventoryStatuses = new[] { "INSTOCK", "LOWSTOCK", "OUTOFSTOCK" };

            for (int i = 1; i <= 50; i++)
            {
                db.Products.Add(new Product
                {
                    Code = $"P-{i:D3}",
                    Name = $"Produit {i}",
                    Description = $"Description du produit {i}",
                    Image = $"image{i}.jpg",
                    Category = categories[(i - 1) % categories.Length],
                    Price = (decimal)(random.Next(10, 200) + random.NextDouble()),
                    Quantity = (i % 7 == 0) ? 0 : random.Next(1, 30),
                    InternalReference = $"INT-{i}",
                    ShellId = i,
                    InventoryStatus = inventoryStatuses[random.Next(inventoryStatuses.Length)],
                    Rating = random.Next(1, 6)
                });
            }
        }

        await db.SaveChangesAsync();
    }
}