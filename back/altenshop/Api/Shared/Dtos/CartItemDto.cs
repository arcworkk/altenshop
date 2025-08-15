using System.ComponentModel.DataAnnotations;

namespace Api.Shared.Dtos;

public class CartItemDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "CartId is required")]
    public int CartId { get; set; }

    [Required(ErrorMessage = "ProductId is required")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
    public ProductDto? Product { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}