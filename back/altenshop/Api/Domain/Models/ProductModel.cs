namespace Api.Domain.Models;

public class ProductModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public decimal Price { get; set; }
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