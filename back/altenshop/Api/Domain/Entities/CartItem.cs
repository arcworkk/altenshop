using Api.Domain.Interfaces;

namespace Api.Domain.Entities;

public class CartItem : IEntityBase
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Cart? Cart { get; set; }
    public Product? Product { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}