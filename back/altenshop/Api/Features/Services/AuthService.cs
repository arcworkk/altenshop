using Api.Domain.Entities;
using Api.Features.Interfaces;
using Api.Infrastructure.Data;
using Api.Shared.Dtos;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Features.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext AppDbContext;
    private readonly IConfiguration IConfiguration;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        AppDbContext = db;
        IConfiguration = config;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        User? user = await AppDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            return null;

        string token = GenerateJwtToken(user);
        return new AuthResponseDto(user.Email, user.Role.ToString(), token);
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequest)
    {
        bool exists = await AppDbContext.Users.AnyAsync(u => u.Email == registerRequest.Email || u.Username == registerRequest.Username);
        if (exists)
            return null;

        var user = new User
        {
            Username = registerRequest.Username,
            Firstname = registerRequest.Firstname,
            Email = registerRequest.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
            Role = registerRequest.Role != null ? (Domain.Enums.UserRole)registerRequest.Role : Domain.Enums.UserRole.User
        };

        AppDbContext.Users.Add(user);
        await AppDbContext.SaveChangesAsync();

        string token = GenerateJwtToken(user);
        return new AuthResponseDto(user.Username, user.Role.ToString(), token);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = IConfiguration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("ConnectedUserId", user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpiresHours"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}