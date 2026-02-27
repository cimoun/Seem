using MediatR;
using Seem.Application.Features.Tags.DTOs;

namespace Seem.Application.Features.Tags.Commands.CreateTag;

public record CreateTagCommand : IRequest<TagDto>
{
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
}
