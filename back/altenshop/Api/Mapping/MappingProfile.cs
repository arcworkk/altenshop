using Api.Domain.Entities;
using Api.Domain.Models;
using Api.Shared.Dtos;
using AutoMapper;

namespace Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product DTO -> Model : Price ?? 0m pour éviter null côté service
        CreateMap<ProductDto, ProductModel>()
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price ?? 0m));
        CreateMap<ProductModel, ProductDto>();

        // Product Model <-> Entity
        CreateMap<ProductModel, Product>().ReverseMap();

        // Cart
        CreateMap<Cart, CartModel>().ReverseMap();
        CreateMap<CartModel, CartDto>().ReverseMap();

        // CartItem
        CreateMap<CartItem, CartItemModel>()
            .ForMember(d => d.Product, o => o.MapFrom(s => s.Product));
        CreateMap<CartItemModel, CartItem>()
            .ForMember(d => d.Product, o => o.Ignore());
        CreateMap<CartItemModel, CartItemDto>().ReverseMap();

        // Wishlist
        CreateMap<Wishlist, WishlistModel>().ReverseMap();
        CreateMap<WishlistModel, WishlistDto>().ReverseMap();

        // WishlistItem
        CreateMap<WishlistItem, WishlistItemModel>()
            .ForMember(d => d.Product, o => o.MapFrom(s => s.Product));
        CreateMap<WishlistItemModel, WishlistItem>()
            .ForMember(d => d.Product, o => o.Ignore());
        CreateMap<WishlistItemModel, WishlistItemDto>().ReverseMap();
    }
}