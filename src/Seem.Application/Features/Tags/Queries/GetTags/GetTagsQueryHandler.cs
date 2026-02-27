using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Tags.DTOs;

namespace Seem.Application.Features.Tags.Queries.GetTags;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, List<TagDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTagsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags
            .OrderBy(t => t.Name)
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                Color = t.Color.HexValue
            })
            .ToListAsync(cancellationToken);

        return tags;
    }
}
