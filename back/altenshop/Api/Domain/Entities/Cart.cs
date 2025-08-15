using Api.Domain.Interfaces;

namespace Api.Domain.Entities;

public class Cart : IEntityBase
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}