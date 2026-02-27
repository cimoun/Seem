using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Auth.DTOs;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken)
            ?? throw new DomainException("Invalid credentials.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Invalid credentials.");

        var token = _tokenService.GenerateToken(user.Id, user.Username, user.Email);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = token
        };
    }
}
