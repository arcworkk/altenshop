namespace Api.Domain.Models;

public class WishlistModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<WishlistItemModel> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}