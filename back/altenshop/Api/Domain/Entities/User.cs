using Api.Domain.Enums;
using Api.Domain.Interfaces;

namespace Api.Domain.Entities;

public class User : IEntityBase
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Firstname { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}