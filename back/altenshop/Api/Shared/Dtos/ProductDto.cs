using System.ComponentModel.DataAnnotations;

namespace Api.Shared.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = default!;

    [Required]
    public string Description { get; set; } = default!;

    [Required, MaxLength(100)]
    public string Category { get; set; } = default!;

    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be > 0")]
    public decimal? Price { get; set; }

    public string? Code { get; set; }
    public string? Image { get; set; }
    public int Quantity { get; set; }
    public string? InternalReference { get; set; }
    public int ShellId { get; set; }
    public string InventoryStatus { get; set; } = "INSTOCK";
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}