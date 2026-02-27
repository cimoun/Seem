using Seem.Domain.Common;

namespace Seem.Domain.Entities.Shared;

public class User : AuditableEntity
{
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? DisplayName { get; private set; }
    public string? AvatarUrl { get; private set; }
    public Dictionary<string, object>? Preferences { get; private set; }

    private User() { }

    public static User Create(string username, string email, string passwordHash, string? displayName = null)
    {
        return new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            DisplayName = displayName
        };
    }

    public void UpdateProfile(string? displayName, string? avatarUrl)
    {
        DisplayName = displayName;
        AvatarUrl = avatarUrl;
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
