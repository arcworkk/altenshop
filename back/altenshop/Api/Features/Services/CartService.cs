using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Infrastructure.Data;
using Api.Shared.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Services;

public class CartService : ICartService
{
    private readonly AppDbContext AppDbContext;
    private readonly IMapper Mapper;

    public CartService(AppDbContext db, IMapper mapper)
    {
        AppDbContext = db;
        Mapper = mapper;
    }

    /// <summary>
    /// Récupère un panier par son Id (inclut les items). Retourne null si introuvable.
    /// </summary>
    public async Task<CartModel?> GetCart(int userId)
    {
        Cart? cart = await AppDbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        return cart?.MapTo<CartModel>(Mapper);
    }

    /// <summary>
    /// Ajoute (ou incrémente) un produit dans le panier de l'utilisateur.
    /// Crée le panier s'il n'existe pas.
    /// </summary>
    public async Task<CartModel> AddCartItem(int userId, int productId, int quantity)
    {
        bool productExists = await AppDbContext.Products
        .AnyAsync(p => p.Id == productId);

        if (!productExists)
            throw new KeyNotFoundException($"Product {productId} not found");

        // Récupère (ou crée) le panier de l'utilisateur
        Cart? cart = await AppDbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            cart = new Cart { UserId = userId };
            AppDbContext.Carts.Add(cart);
        }

        // Cherche l'item existant pour ce produit
        CartItem? item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity += quantity;
        }

        await AppDbContext.SaveChangesAsync();

        // Recharge pour renvoyer un état complet/à jour
        cart = await AppDbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstAsync(c => c.Id == cart.Id);

        return cart.MapTo<CartModel>(Mapper);
    }

    /// <summary>
    /// Met à jour la quantité d'un produit dans le panier de l'utilisateur.
    /// Retourne null si le panier ou l'item n'existent pas.
    /// </summary>
    public async Task<CartModel?> UpdateCartItem(int userId, int productId, int quantity)
    {
        Cart? cart = await AppDbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
            return null;

        CartItem? item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
            return null;

        item.Quantity = quantity;

        await AppDbContext.SaveChangesAsync();

        // Retourne l'état à jour
        return cart.MapTo<CartModel>(Mapper);
    }

    /// <summary>
    /// Supprime un produit du panier. Retourne true si supprimé.
    /// </summary>
    public async Task<bool> DeleteCartItem(int userId, int productId)
    {
        var cartItem = await AppDbContext.CartItems
            .Include(c => c.Cart)
            .FirstOrDefaultAsync(c => c.Cart!.UserId == userId);

        if (cartItem is null)
            return false;

        AppDbContext.CartItems.Remove(cartItem);
        await AppDbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Supprime un panier et tous ses items par Id. Retourne true si supprimé.
    /// </summary>
    public async Task<bool> DeleteCart(int userId)
    {
        var cart = await AppDbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
            return false;

        AppDbContext.Carts.Remove(cart);
        await AppDbContext.SaveChangesAsync();
        return true;
    }
}
