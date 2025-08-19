using Api.Common;
using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Infrastructure.Data;
using Api.Shared.Dtos;
using Api.Shared.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Services;

/// <summary>
/// Service gérant la logique métier et l'accès aux données des produits.
/// </summary>
public class ProductService : IProductService
{
    private readonly AppDbContext AppDbContext;

    private readonly IMapper Mapper;

    public ProductService(AppDbContext db, IMapper mapper)
    {
        AppDbContext = db;
        Mapper = mapper;
    }

    /// <summary>
    /// Récupère une liste paginée de produits, avec option de recherche.
    /// </summary>
    public async Task<PaginatedResult<ProductModel>> GetPaginatedProduct(int page, int pageSize, string? search, string? filter, string? sort)
    {
        // parse filtre
        var productFilterDto = string.IsNullOrWhiteSpace(filter)
            ? null
            : System.Text.Json.JsonSerializer.Deserialize<ProductFilterDto>(filter);

        IQueryable<Product> query = AppDbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

        if (productFilterDto is not null)
        {
            if (!string.IsNullOrWhiteSpace(productFilterDto.Category) && productFilterDto.Category != "All")
                query = query.Where(p => p.Category == productFilterDto.Category);

            if (productFilterDto.InventoryStatus is { Length: > 0 })
                query = query.Where(p => productFilterDto.InventoryStatus.Contains(p.InventoryStatus));

            if (productFilterDto.PriceMin.HasValue)
                query = query.Where(p => p.Price >= productFilterDto.PriceMin.Value);

            if (productFilterDto.PriceMax.HasValue)
                query = query.Where(p => p.Price <= productFilterDto.PriceMax.Value);

            if (productFilterDto.RatingMin.HasValue)
                query = query.Where(p => p.Rating >= productFilterDto.RatingMin.Value);

            if (productFilterDto.RatingMax.HasValue)
                query = query.Where(p => p.Rating <= productFilterDto.RatingMax.Value);
        }

        var sortField = "id";
        var sortDir = "asc";

        // parse du tri
        if (!string.IsNullOrWhiteSpace(sort))
        {
            var parts = sort.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length > 0) sortField = parts[0].ToLowerInvariant();
            if (parts.Length > 1) sortDir = parts[1].ToLowerInvariant() == "desc" ? "desc" : "asc";
        }

        query = (sortField, sortDir) switch
        {
            ("price", "asc") => query.OrderBy(p => p.Price),
            ("price", "desc") => query.OrderByDescending(p => p.Price),

            ("name", "asc") => query.OrderBy(p => p.Name),
            ("name", "desc") => query.OrderByDescending(p => p.Name),

            ("category", "asc") => query.OrderBy(p => p.Category),
            ("category", "desc") => query.OrderByDescending(p => p.Category),

            ("rating", "asc") => query.OrderBy(p => p.Rating),
            ("rating", "desc") => query.OrderByDescending(p => p.Rating),

            ("createdat", "asc") => query.OrderBy(p => p.CreatedAt),
            ("createdat", "desc") => query.OrderByDescending(p => p.CreatedAt),

            ("updatedat", "asc") => query.OrderBy(p => p.UpdatedAt),
            ("updatedat", "desc") => query.OrderByDescending(p => p.UpdatedAt),

            ("id", "desc") => query.OrderByDescending(p => p.Id),
            _ => query.OrderBy(p => p.Id) // défaut
        };

        int total = await query.CountAsync();
        List<Product> products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<ProductModel>(products.MapTo<List<ProductModel>>(Mapper), total, page, pageSize);
    }

    /// <summary>
    /// Récupère un produit via son identifiant.
    /// </summary>
    public async Task<ProductModel?> GetProduct(int id)
    {
        Product? product = await AppDbContext.Products.FindAsync(id);

        return product?.MapTo<ProductModel>(Mapper) ;
    }

    /// <summary>
    /// Crée un nouveau produit.
    /// </summary>
    public async Task<ProductModel> CreateProduct(ProductModel productModel)
    {
        Product product = productModel.MapTo<Product>(Mapper);

        AppDbContext.Products.Add(product);

        await AppDbContext.SaveChangesAsync();

        return product.MapTo<ProductModel>(Mapper);
    }

    /// <summary>
    /// Met à jour un produit existant.
    /// </summary>
    public async Task<ProductModel?> UpdateProduct(ProductModel productModel)
    {
        Product? product = await AppDbContext.Products.FindAsync(productModel.Id);
        if (product == null)
            return null;

        productModel.MapInto(product, Mapper);

        await AppDbContext.SaveChangesAsync();
        
        return product.MapTo<ProductModel>(Mapper);
    }

    /// <summary>
    /// Supprime un produit via son identifiant.
    /// </summary>
    public async Task<bool> DeleteProduct(int id)
    {
        Product? product = await AppDbContext.Products.FindAsync(id);
        if (product == null)
            return false;

        AppDbContext.Products.Remove(product);
        await AppDbContext.SaveChangesAsync();
        return true;
    }
}