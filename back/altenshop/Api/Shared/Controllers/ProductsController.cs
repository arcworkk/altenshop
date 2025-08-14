using Api.Common;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Shared.Dtos;
using Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Shared.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService IProductService;

    public ProductsController(IProductService svc)
    {
        IProductService = svc;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResult<PaginedResult<ProductDto>>>> GetAllProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null)
    {
        PaginedResult<ProductModel> pagedProductModels = await IProductService.GetPaginedProduct(page, pageSize, q);

        List<ProductDto> productDtos = pagedProductModels.Items
            .Select((ProductModel productModel) => productModel.ToDto())
            .ToList();

        var dtoPage = new PaginedResult<ProductDto>(
            productDtos,
            pagedProductModels.Total,
            pagedProductModels.Page,
            pagedProductModels.PageSize
        );

        return Ok(ApiResult<PaginedResult<ProductDto>>.Ok(dtoPage));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResult<ProductDto>>> GetProduct(int id)
    {
        ProductModel? productModel = await IProductService.GetProduct(id);
        if (productModel is null)
            return NotFound(ApiResult<ProductDto>.Fail("Not found"));

        ProductDto productDto = productModel.ToDto();
        return Ok(ApiResult<ProductDto>.Ok(productDto));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<ApiResult<ProductDto>>> CreateProduct([FromBody] ProductDto productDto)
    {
        ProductModel productModel = productDto.ToModel();
        ProductModel createdProductModel = await IProductService.CreateProduct(productModel);
        ProductDto createdProductDto = createdProductModel.ToDto();

        return CreatedAtAction(nameof(GetProduct),
            new { id = createdProductDto.Id },
            ApiResult<ProductDto>.Ok(createdProductDto));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResult<ProductDto>>> UpdateProduct(int id, [FromBody] ProductDto productDto)
    {
        ProductModel productModel = productDto.ToModel();
        ProductModel? updatedProductModel = await IProductService.UpdateProduct(id, productModel);
        if (updatedProductModel is null)
            return NotFound(ApiResult<ProductDto>.Fail("Not found"));

        ProductDto updatedProductDto = updatedProductModel.ToDto();
        return Ok(ApiResult<ProductDto>.Ok(updatedProductDto));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResult<bool>>> DeleteProduct(int id)
    {
        bool deleted = await IProductService.UpdateProduct(id);
        return deleted
            ? Ok(ApiResult<bool>.Ok(true))
            : NotFound(ApiResult<bool>.Fail("Not found"));
    }
}