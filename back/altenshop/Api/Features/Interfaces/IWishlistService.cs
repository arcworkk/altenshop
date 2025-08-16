using Api.Domain.Models;

namespace Api.Features.Interfaces;

public interface IWishlistService
{
    Task<WishlistModel> GetWishlist(int userId);
    Task<WishlistModel> AddWishlistItem(int userId, int productId);
    Task<WishlistModel?> UpdateWishlistItem(int userId, int productId);
    Task<bool> DeleteWishlist(int userId);
}