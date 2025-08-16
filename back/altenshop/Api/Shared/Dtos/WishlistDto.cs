using System.ComponentModel.DataAnnotations;

namespace Api.Shared.Dtos;

public class WishlistDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public int UserId { get; set; }
    public List<WishlistItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}