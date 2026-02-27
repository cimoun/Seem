using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Auth.DTOs;
using Seem.Domain.Entities.Shared;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            if (existingUser.Username == request.Username)
                throw new DomainException("Username is already taken.");
            throw new DomainException("Email is already registered.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = User.Create(request.Username, request.Email, passwordHash, request.DisplayName);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

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
