using Api.Domain.Interfaces;

namespace Api.Domain.Entities;

public class WishlistItem : IEntityBase
{
    public int Id { get; set; }
    public int WishlistId { get; set; }
    public int ProductId { get; set; }
    public Wishlist? Wishlist { get; set; }
    public Product? Product { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}