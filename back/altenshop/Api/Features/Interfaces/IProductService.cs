using Api.Common;
using Api.Domain.Models;

namespace Api.Features.Interfaces;

public interface IProductService
{
    Task<PaginedResult<ProductModel>> GetPaginedProduct(int page, int pageSize, string? search);
    Task<ProductModel?> GetProduct(int id);
    Task<ProductModel> CreateProduct(ProductModel productModel);
    Task<ProductModel?> UpdateProduct(int id, ProductModel productModel);
    Task<bool> UpdateProduct(int id);
}
