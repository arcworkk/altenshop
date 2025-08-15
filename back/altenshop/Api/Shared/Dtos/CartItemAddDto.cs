using System.ComponentModel.DataAnnotations;

namespace Api.Shared.Dtos;

public class CartItemAddDto
{
    [Required(ErrorMessage = "ProductId is required")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
