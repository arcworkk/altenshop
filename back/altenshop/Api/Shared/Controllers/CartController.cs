using Api.Common;
using Api.Domain.Models;
using Api.Features.Interfaces;
using Api.Shared.Dtos;
using Api.Shared.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Shared.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD du panier client.
/// </summary>
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

    /// <summary>
    /// Récupère le panier de l'utilisateur connecté (inclut les items et les product). Créer et retourne un nouveau panier si introuvable.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResult<CartDto>>> GetCart()
    {
        CartModel cartModel = await CartService.GetCart(GetConnectedUserId());

        return Ok(ApiResult<CartDto>.Ok(cartModel.MapTo<CartDto>(Mapper)));
    }

    /// <summary>
    /// Ajoute (ou incrémente) un produit dans le panier de l'utilisateur connecté.
    /// Crée le panier s'il n'existe pas.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResult<CartDto>>> AddCartItem([FromBody] CartItemAddDto cartItemAddDto)
    {
        CartModel createdCartModel = await CartService.AddCartItem(GetConnectedUserId(), cartItemAddDto.ProductId, cartItemAddDto.Quantity);

        return Ok(ApiResult<CartDto>.Ok(createdCartModel.MapTo<CartDto>(Mapper)));
    }

    /// <summary>
    /// Met à jour la quantité d'un produit dans le panier de l'utilisateur connecté.
    /// Retourne null si le panier ou l'item n'existent pas.
    /// </summary>
    [HttpPut("{productId:int}")]
    public async Task<ActionResult<ApiResult<CartDto>>> UpdateCartItem(int productId, [FromBody] CartItemUpdateDto cartItemUpdateDto)
    {
        CartModel? updatedCartModel = await CartService.UpdateCartItem(GetConnectedUserId(), productId, cartItemUpdateDto.Quantity);
        if (updatedCartModel is null)
            return NotFound(ApiResult<CartDto>.Fail("Not found"));

        return Ok(ApiResult<CartDto>.Ok(updatedCartModel.MapTo<CartDto>(Mapper)));
    }

    /// <summary>
    /// Supprime un produit du panier de l'utilisateur connecté.
    /// </summary>
    [HttpDelete("{productId:int}")]
    public async Task<ActionResult<ApiResult<bool>>> DeleteCartItem(int productId)
    {
        bool deleted = await CartService.DeleteCartItem(GetConnectedUserId(), productId);
        return deleted
            ? Ok(ApiResult<bool>.Ok(true))
            : NotFound(ApiResult<bool>.Fail("Not found"));
    }

    /// <summary>
    /// Supprime le panier de l'utilisateur connecté et tous ses produit.
    /// </summary>
    [HttpDelete]
    public async Task<ActionResult<ApiResult<bool>>> DeleteCart()
    {
        bool deleted = await CartService.DeleteCart(GetConnectedUserId());
        return deleted
            ? Ok(ApiResult<bool>.Ok(true))
            : NotFound(ApiResult<bool>.Fail("Not found"));
    }
}