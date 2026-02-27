using MediatR;
using Seem.Application.Features.Auth.DTOs;

namespace Seem.Application.Features.Auth.Commands.Login;

public record LoginCommand : IRequest<AuthResponseDto>
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}
