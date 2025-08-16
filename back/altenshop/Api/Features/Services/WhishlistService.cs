using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Infrastructure.Data;
using Api.Shared.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Services;

/// <summary>
/// Service gérant la logique métier et l'accès aux données de la liste d'envie client.
/// </summary>
public class WishlistService : IWishlistService
{
    private readonly AppDbContext AppDbContext;
    private readonly IMapper Mapper;

    public WishlistService(AppDbContext db, IMapper mapper)
    {
        AppDbContext = db;
        Mapper = mapper;
    }

    /// <summary>
    /// Récupère la liste d'envie de l'utilisateur connecté (inclut les items et les product). Créer et retourne une nouvelle liste d'envie si introuvable.
    /// </summary>
    public async Task<WishlistModel> GetWishlist(int userId)
    {
        Wishlist? wishlist = await AppDbContext.Wishlists
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        
        if (wishlist is null)
        {
            wishlist = new Wishlist { UserId = userId };
            AppDbContext.Wishlists.Add(wishlist);
            await AppDbContext.SaveChangesAsync();
        }

        return wishlist.MapTo<WishlistModel>(Mapper);
    }

    /// <summary>
    /// Ajoute un produit dans la liste d'envie de l'utilisateur connecté.
    /// Crée la liste d'envie s'il n'existe pas.
    /// </summary>
    public async Task<WishlistModel> AddWishlistItem(int userId, int productId)
    {
        bool productExists = await AppDbContext.Products
        .AnyAsync(p => p.Id == productId);

        if (!productExists)
            throw new KeyNotFoundException($"Product {productId} not found");

        // Récupère (ou crée) la liste d'envie de l'utilisateur
        Wishlist? wishlist = await AppDbContext.Wishlists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (wishlist is null)
        {
            wishlist = new Wishlist { UserId = userId };
            AppDbContext.Wishlists.Add(wishlist);
        }

        // Cherche l'item existant pour ce produit
        WishlistItem? item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            wishlist.Items.Add(new WishlistItem
            {
                ProductId = productId,
            });
        }

        await AppDbContext.SaveChangesAsync();

        // Recharge pour renvoyer un état complet/à jour
        wishlist = await AppDbContext.Wishlists
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstAsync(c => c.Id == wishlist.Id);

        return wishlist.MapTo<WishlistModel>(Mapper);
    }

    /// <summary>
    /// Met à jour un produit dans la liste d'envie de l'utilisateur connecté.
    /// Retourne null si la liste d'envie ou l'item n'existent pas.
    /// </summary>
    public async Task<WishlistModel?> UpdateWishlistItem(int userId, int productId)
    {
        Wishlist? wishlist = await AppDbContext.Wishlists
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (wishlist is null)
            return null;

        // Cherche l'item existant pour ce produit
        WishlistItem? item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            wishlist.Items.Add(new WishlistItem
            {
                ProductId = productId,
            });
        }
        else
        {
            wishlist.Items.Remove(item);
        }

        await AppDbContext.SaveChangesAsync();

        // Recharge pour renvoyer un état complet/à jour
        wishlist = await AppDbContext.Wishlists
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstAsync(c => c.Id == wishlist.Id);

        return wishlist.MapTo<WishlistModel>(Mapper);
    }

    /// <summary>
    /// Supprime la liste d'envie de l'utilisateur connecté et tous ses produit.
    /// </summary>
    public async Task<bool> DeleteWishlist(int userId)
    {
        var wishlist = await AppDbContext.Wishlists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (wishlist is null)
            return false;

        AppDbContext.Wishlists.Remove(wishlist);
        await AppDbContext.SaveChangesAsync();
        return true;
    }
}
