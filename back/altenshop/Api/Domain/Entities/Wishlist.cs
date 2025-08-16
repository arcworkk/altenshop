using Api.Domain.Interfaces;

namespace Api.Domain.Entities;

public class Wishlist : IEntityBase
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}