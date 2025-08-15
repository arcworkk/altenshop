using System.ComponentModel.DataAnnotations;

namespace Api.Shared.Dtos;

public class CartItemUpdateDto
{
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
