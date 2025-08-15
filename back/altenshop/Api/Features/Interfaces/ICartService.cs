using Api.Domain.Models;

namespace Api.Features.Interfaces;

public interface ICartService
{
    Task<CartModel> GetCart(int userId);
    Task<CartModel> AddCartItem(int userId, int productId, int quantity);
    Task<CartModel?> UpdateCartItem(int userId, int productId, int quantity);
    Task<bool> DeleteCartItem(int userId, int productId);
    Task<bool> DeleteCart(int userId);
}