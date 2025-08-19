using System.Text.Json.Serialization;

namespace Api.Shared.Dtos;

public class ProductFilterDto
{
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("inventoryStatus")]
    public string[]? InventoryStatus { get; set; }

    [JsonPropertyName("priceMin")]
    public decimal? PriceMin { get; set; }

    [JsonPropertyName("priceMax")]
    public decimal? PriceMax { get; set; }

    [JsonPropertyName("ratingMin")]
    public int? RatingMin { get; set; }

    [JsonPropertyName("ratingMax")]
    public int? RatingMax { get; set; }
}