namespace Seem.Application.Features.Tags.DTOs;

public record TagDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
}
