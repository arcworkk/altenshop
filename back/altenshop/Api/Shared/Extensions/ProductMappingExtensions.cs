using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Shared.Dtos;

namespace Api.Shared.Extensions;

public static class ProductMappingExtensions
{
    // Entity -> DTO
    public static ProductDto ToDto(this Product entity)
    {
        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Category = entity.Category,
            Price = entity.Price,
            Code = entity.Code,
            Image = entity.Image,
            Quantity = entity.Quantity,
            InternalReference = entity.InternalReference,
            ShellId = entity.ShellId,
            InventoryStatus = entity.InventoryStatus,
            Rating = entity.Rating,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    // DTO -> Model
    public static ProductModel ToModel(this ProductDto dto)
    {
        return new ProductModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            Price = dto.Price ?? 0m, // gère le null côté DTO
            Code = dto.Code,
            Image = dto.Image,
            Quantity = dto.Quantity,
            InternalReference = dto.InternalReference,
            ShellId = dto.ShellId,
            InventoryStatus = dto.InventoryStatus,
            Rating = dto.Rating,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    // Model -> Entity
    public static Product ToEntity(this ProductModel model)
    {
        return new Product
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Category = model.Category,
            Price = model.Price,
            Code = model.Code,
            Image = model.Image,
            Quantity = model.Quantity,
            InternalReference = model.InternalReference,
            ShellId = model.ShellId,
            InventoryStatus = model.InventoryStatus,
            Rating = model.Rating,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt
        };
    }

    // Entity -> Model
    public static ProductModel ToModel(this Product entity)
    {
        return new ProductModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Category = entity.Category,
            Price = entity.Price,
            Code = entity.Code,
            Image = entity.Image,
            Quantity = entity.Quantity,
            InternalReference = entity.InternalReference,
            ShellId = entity.ShellId,
            InventoryStatus = entity.InventoryStatus,
            Rating = entity.Rating,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    // Model -> DTO
    public static ProductDto ToDto(this ProductModel model)
    {
        return new ProductDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Category = model.Category,
            Price = model.Price,
            Code = model.Code,
            Image = model.Image,
            Quantity = model.Quantity,
            InternalReference = model.InternalReference,
            ShellId = model.ShellId,
            InventoryStatus = model.InventoryStatus,
            Rating = model.Rating,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt
        };
    }
}