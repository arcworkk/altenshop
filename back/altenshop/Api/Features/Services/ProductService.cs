using Api.Common;
using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Infrastructure.Data;
using Api.Shared.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext AppDbContext;

    private readonly IMapper Mapper;

    public ProductService(AppDbContext db, IMapper mapper)
    {
        AppDbContext = db;
        Mapper = mapper;
    }

    public async Task<PaginatedResult<ProductModel>> GetPaginatedProduct(int page, int pageSize, string? search)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        IQueryable<Product> query = AppDbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(product => product.Name.Contains(search) || product.Description.Contains(search));

        int total = await query.CountAsync();
        List<Product> products = await query
            .OrderBy(product => product.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<ProductModel>(products.MapTo<List<ProductModel>>(Mapper), total, page, pageSize);
    }

    public async Task<ProductModel?> GetProduct(int id)
    {
        Product? product = await AppDbContext.Products.FindAsync(id);

        return product?.MapTo<ProductModel>(Mapper) ;
    }

    public async Task<ProductModel> CreateProduct(ProductModel productModel)
    {
        Product product = productModel.MapTo<Product>(Mapper);

        AppDbContext.Products.Add(product);

        await AppDbContext.SaveChangesAsync();

        return product.MapTo<ProductModel>(Mapper);
    }

    public async Task<ProductModel?> UpdateProduct(ProductModel productModel)
    {
        Product? product = await AppDbContext.Products.FindAsync(productModel.Id);
        if (product == null)
            return null;

        productModel.MapInto(product, Mapper);

        await AppDbContext.SaveChangesAsync();
        
        return product.MapTo<ProductModel>(Mapper);
    }

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