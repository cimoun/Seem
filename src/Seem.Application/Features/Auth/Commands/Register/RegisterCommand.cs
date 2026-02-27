using MediatR;
using Seem.Application.Features.Auth.DTOs;

namespace Seem.Application.Features.Auth.Commands.Register;

public record RegisterCommand : IRequest<AuthResponseDto>
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string? DisplayName { get; init; }
}
