using Api.Common;
using Api.Domain.Models;

namespace Api.Features.Interfaces;

public interface IProductService
{
    Task<PaginatedResult<ProductModel>> GetPaginatedProduct(int page, int pageSize, string? search);
    Task<ProductModel?> GetProduct(int id);
    Task<ProductModel> CreateProduct(ProductModel productModel);
    Task<ProductModel?> UpdateProduct(ProductModel productModel);
    Task<bool> DeleteProduct(int id);
}
