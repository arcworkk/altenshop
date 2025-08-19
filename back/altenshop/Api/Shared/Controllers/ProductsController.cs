using Api.Common;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Shared.Dtos;
using Api.Shared.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Shared.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD des produits.
/// </summary>
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService ProductService;
    private readonly IMapper Mapper;

    public ProductsController(IProductService productService, IMapper mapper)
    {
        ProductService = productService;
        Mapper = mapper;
    }

    /// <summary>
    /// Récupère une liste paginée de produits, avec option de recherche.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResult<PaginatedResult<ProductDto>>>> GetAllProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery]string? q = null,
            [FromQuery]string? filter = null,
            [FromQuery]string? sort = null)
    {
        PaginatedResult<ProductModel> paginatedProductsModel = await ProductService.GetPaginatedProduct(page, pageSize, q, filter, sort);

        List<ProductDto> productDto = paginatedProductsModel.Items
            .Select((ProductModel productModel) => productModel.MapTo<ProductDto>(Mapper))
            .ToList();

        PaginatedResult<ProductDto> paginatedResult = new PaginatedResult<ProductDto>(
            productDto,
            paginatedProductsModel.Total,
            paginatedProductsModel.Page,
            paginatedProductsModel.PageSize
        );

        return Ok(ApiResult<PaginatedResult<ProductDto>>.Ok(paginatedResult));
    }

    /// <summary>
    /// Récupère un produit via son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResult<ProductDto>>> GetProduct(int id)
    {
        ProductModel? productModel = await ProductService.GetProduct(id);
        if (productModel is null)
            return NotFound(ApiResult<ProductDto>.Fail("Not found"));

        return Ok(ApiResult<ProductDto>.Ok(productModel.MapTo<ProductDto>(Mapper)));
    }

    /// <summary>
    /// Crée un nouveau produit.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<ApiResult<ProductDto>>> CreateProduct([FromBody] ProductDto productDto)
    {
        ProductModel productModel = productDto.MapTo<ProductModel>(Mapper);
        ProductModel createdProductModel = await ProductService.CreateProduct(productModel);
        ProductDto createdProductDto = createdProductModel.MapTo<ProductDto>(Mapper);

        return CreatedAtAction(nameof(GetProduct),
            new { id = createdProductDto.Id },
            ApiResult<ProductDto>.Ok(createdProductDto));
    }

    /// <summary>
    /// Met à jour un produit existant.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResult<ProductDto>>> UpdateProduct(int id, [FromBody] ProductDto productDto)
    {
        ProductModel productModel = productDto.MapTo<ProductModel>(Mapper);
        productModel.Id = id;
        ProductModel? updatedProductModel = await ProductService.UpdateProduct(productModel);
        if (updatedProductModel is null)
            return NotFound(ApiResult<ProductDto>.Fail("Not found"));

        ProductDto updatedProductDto = updatedProductModel.MapTo<ProductDto>(Mapper);
        return Ok(ApiResult<ProductDto>.Ok(updatedProductDto));
    }

    /// <summary>
    /// Supprime un produit via son identifiant.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResult<bool>>> DeleteProduct(int id)
    {
        bool deleted = await ProductService.DeleteProduct(id);
        return deleted
            ? Ok(ApiResult<bool>.Ok(true))
            : NotFound(ApiResult<bool>.Fail("Not found"));
    }
}