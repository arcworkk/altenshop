using Api.Shared.Dtos;

namespace Api.Domain.Models;

public class WishlistItemModel
{
    public int Id { get; set; }
    public int WishlistId { get; set; }
    public int ProductId { get; set; }
    public ProductModel? Product { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}