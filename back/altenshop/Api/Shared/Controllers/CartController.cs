using Api.Common;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Shared.Dtos;
using Api.Shared.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Shared.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService CartService;
    private readonly IMapper Mapper;

    public CartController(ICartService cartService, IMapper mapper)
    {
        CartService = cartService;
        Mapper = mapper;
    }

    private int GetConnectedUserId() => int.Parse(User.FindFirstValue("ConnectedUserId")!);

    [HttpGet]
    public async Task<ActionResult<ApiResult<CartDto>>> GetCart()
    {
        CartModel? cartModel = await CartService.GetCart(GetConnectedUserId());
        if (cartModel is null)
            return NotFound(ApiResult<CartDto>.Fail("Not found"));

        return Ok(ApiResult<CartDto>.Ok(cartModel.MapTo<CartDto>(Mapper)));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResult<CartDto>>> AddCartItem([FromBody] CartItemAddDto cartItemAddDto)
    {
        CartModel createdCartModel = await CartService.AddCartItem(GetConnectedUserId(), cartItemAddDto.ProductId, cartItemAddDto.Quantity);

        return Ok(ApiResult<CartDto>.Ok(createdCartModel.MapTo<CartDto>(Mapper)));
    }

    [HttpPut("{productId:int}")]
    public async Task<ActionResult<ApiResult<CartDto>>> UpdateCartItem(int productId, [FromBody] CartItemUpdateDto cartItemUpdateDto)
    {
        CartModel? updatedCartModel = await CartService.UpdateCartItem(GetConnectedUserId(), productId, cartItemUpdateDto.Quantity);
        if (updatedCartModel is null)
            return NotFound(ApiResult<CartDto>.Fail("Not found"));

        return Ok(ApiResult<CartDto>.Ok(updatedCartModel.MapTo<CartDto>(Mapper)));
    }

    [HttpDelete("{productId:int}")]
    public async Task<ActionResult<ApiResult<bool>>> DeleteCartItem(int productId)
    {
        bool deleted = await CartService.DeleteCartItem(GetConnectedUserId(), productId);
        return deleted
            ? Ok(ApiResult<bool>.Ok(true))
            : NotFound(ApiResult<bool>.Fail("Not found"));
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResult<bool>>> DeleteCart()
    {
        bool deleted = await CartService.DeleteCart(GetConnectedUserId());
        return deleted
            ? Ok(ApiResult<bool>.Ok(true))
            : NotFound(ApiResult<bool>.Fail("Not found"));
    }
}