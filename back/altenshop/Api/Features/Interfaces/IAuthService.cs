using Api.Shared.Dtos;

namespace Api.Features.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequest);
}