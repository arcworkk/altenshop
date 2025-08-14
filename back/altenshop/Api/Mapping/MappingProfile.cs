using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Shared.Dtos;
using AutoMapper;

namespace Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // DTO -> Model : Price ?? 0m pour éviter null côté service
        CreateMap<ProductDto, ProductModel>()
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price ?? 0m));
        CreateMap<ProductModel, ProductDto>();

        // Model <-> Entity
        CreateMap<ProductModel, Product>().ReverseMap();
    }
}