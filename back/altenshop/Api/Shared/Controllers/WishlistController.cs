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
/// Contrôleur gérant les opérations CRUD de la liste d'envie client.
/// </summary>
[ApiController]
[Route("api/wishlist")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService WishlistService;
    private readonly IMapper Mapper;

    public WishlistController(IWishlistService wishlistService, IMapper mapper)
    {
        WishlistService = wishlistService;
        Mapper = mapper;
    }

    private int GetConnectedUserId() => int.Parse(User.FindFirstValue("ConnectedUserId")!);

    /// <summary>
    /// Récupère la liste d'envie de l'utilisateur connecté (inclut les items et les product). Créer et retourne une nouvelle liste d'envie si introuvable.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResult<WishlistDto>>> GetWishlist()
    {
        WishlistModel wishlistModel = await WishlistService.GetWishlist(GetConnectedUserId());

        return Ok(ApiResult<WishlistDto>.Ok(wishlistModel.MapTo<WishlistDto>(Mapper)));
    }

    /// <summary>
    /// Ajoute un produit dans la liste d'envie de l'utilisateur connecté.
    /// Crée la liste d'envie s'il n'existe pas.
    /// </summary>
    [HttpPost("{productId:int}")]
    public async Task<ActionResult<ApiResult<WishlistDto>>> AddWishlistItem(int productId)
    {
        WishlistModel createdWishlistModel = await WishlistService.AddWishlistItem(GetConnectedUserId(), productId);

        return Ok(ApiResult<WishlistDto>.Ok(createdWishlistModel.MapTo<WishlistDto>(Mapper)));
    }

    /// <summary>
    /// Met à jour un produit dans la liste d'envie de l'utilisateur connecté.
    /// Retourne null si la liste d'envie ou l'item n'existent pas.
    /// </summary>
    [HttpPut("{productId:int}")]
    public async Task<ActionResult<ApiResult<WishlistDto>>> UpdateWishlistItem(int productId)
    {
        WishlistModel? updatedWishlistModel = await WishlistService.UpdateWishlistItem(GetConnectedUserId(), productId);
        if (updatedWishlistModel is null)
            return NotFound(ApiResult<WishlistDto>.Fail("Not found"));

        return Ok(ApiResult<WishlistDto>.Ok(updatedWishlistModel.MapTo<WishlistDto>(Mapper)));
    }

    /// <summary>
    /// Supprime la liste d'envie de l'utilisateur connecté et tous ses produit.
    /// </summary>
    [HttpDelete]
    public async Task<ActionResult<ApiResult<bool>>> DeleteWishlist()
    {
        bool deleted = await WishlistService.DeleteWishlist(GetConnectedUserId());
        return deleted
            ? Ok(ApiResult<bool>.Ok(true))
            : NotFound(ApiResult<bool>.Fail("Not found"));
    }
}