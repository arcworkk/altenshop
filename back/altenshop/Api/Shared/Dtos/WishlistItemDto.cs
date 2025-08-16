using System.ComponentModel.DataAnnotations;

namespace Api.Shared.Dtos;

public class WishlistItemDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "WishlistId is required")]
    public int WishlistId { get; set; }

    [Required(ErrorMessage = "ProductId is required")]
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
