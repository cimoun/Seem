namespace Seem.Application.Features.Auth.DTOs;

public record AuthResponseDto
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? DisplayName { get; init; }
    public string Token { get; init; } = null!;
}
