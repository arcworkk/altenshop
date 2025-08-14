using Api.Common;
using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Infrastructure.Data;
using Api.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext AppDbContext;

    public ProductService(AppDbContext db)
    {
        AppDbContext = db;
    }

    public async Task<PaginedResult<ProductModel>> GetPaginedProduct(int page, int pageSize, string? search)
    {
        var query = AppDbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(product => product.Name.Contains(search) || product.Description.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(product => product.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productModels = items.Select(product => product.ToModel()).ToList();

        return new PaginedResult<ProductModel>(productModels, total, page, pageSize);
    }

    public async Task<ProductModel?> GetProduct(int id)
    {
        Product? product = await AppDbContext.Products.FindAsync(id);
        return product?.ToModel();
    }

    public async Task<ProductModel> CreateProduct(ProductModel productModel)
    {
        Product product = productModel.ToEntity();
        AppDbContext.Products.Add(product);
        await AppDbContext.SaveChangesAsync();
        return product.ToModel();
    }

    public async Task<ProductModel?> UpdateProduct(int id, ProductModel productModel)
    {
        Product? product = await AppDbContext.Products.FindAsync(id);
        if (product == null)
            return null;

        // mise à jour manuelle
        product.Name = productModel.Name;
        product.Description = productModel.Description;
        product.Category = productModel.Category;
        product.Price = productModel.Price;
        product.Code = productModel.Code;
        product.Image = productModel.Image;
        product.Quantity = productModel.Quantity;
        product.InternalReference = productModel.InternalReference;
        product.ShellId = productModel.ShellId;
        product.InventoryStatus = productModel.InventoryStatus;
        product.Rating = productModel.Rating;

        await AppDbContext.SaveChangesAsync();
        return product.ToModel();
    }

    public async Task<bool> UpdateProduct(int id)
    {
        Product? product = await AppDbContext.Products.FindAsync(id);
        if (product == null)
            return false;

        AppDbContext.Products.Remove(product);
        await AppDbContext.SaveChangesAsync();
        return true;
    }
}