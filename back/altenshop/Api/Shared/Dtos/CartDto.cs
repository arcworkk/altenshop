using System.ComponentModel.DataAnnotations;

namespace Api.Shared.Dtos;

public class CartDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public int UserId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}