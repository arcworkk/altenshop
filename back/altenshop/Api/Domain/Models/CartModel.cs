namespace Api.Domain.Models;

public class CartModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItemModel> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}