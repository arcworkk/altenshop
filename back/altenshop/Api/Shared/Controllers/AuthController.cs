using Api.Common;
using Api.Features.Interfaces;
using Api.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Shared.Controllers;

/// <summary>
/// Contrôleur gérant l'authentification.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService IAuthService;

    public AuthController(IAuthService auth)
    {
        IAuthService = auth;
    }

    /// <summary>
    /// Authentifie un utilisateur avec son email et mot de passe puis génère son token JWT.
    /// </summary>
    [HttpPost("token")]
    public async Task<ActionResult<ApiResult<LoginRequestDto>>> Login([FromBody] LoginRequestDto loginRequest)
    {
        AuthResponseDto? res = await IAuthService.LoginAsync(loginRequest);
        return res is null
            ? Unauthorized(ApiResult<AuthResponseDto>.Fail("Invalid credentials"))
            : Ok(ApiResult<AuthResponseDto>.Ok(res));
    }

    /// <summary>
    /// Créer un utilisateur puis génère son token JWT.
    /// </summary>
    [HttpPost("account")]
    public async Task<ActionResult<ApiResult<AuthResponseDto>>> Register([FromBody] RegisterRequestDto registerRequest)
    {
        AuthResponseDto? res = await IAuthService.RegisterAsync(registerRequest);
        return res is null
            ? Conflict(ApiResult<AuthResponseDto>.Fail("Email or Username already exists"))
            : Ok(ApiResult<AuthResponseDto>.Ok(res));
    }
}