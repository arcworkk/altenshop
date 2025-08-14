namespace Api.Shared.Dtos;

public record LoginRequestDto(string Email, string Password);
public record AuthResponseDto(string Email, string Role, string Token);
public record RegisterRequestDto(string Username, string Firstname, string Email, string Password, int? Role);