using MediatR;
using Seem.Application.Features.Tags.DTOs;

namespace Seem.Application.Features.Tags.Queries.GetTags;

public record GetTagsQuery : IRequest<List<TagDto>>;
