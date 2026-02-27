using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Projects.DTOs;

namespace Seem.Application.Features.Projects.Queries.ListProjects;

public class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context;

    public ListProjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectDto>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .Where(p => !p.IsArchived)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Key = p.Key,
                Color = p.Color.HexValue,
                IsArchived = p.IsArchived,
                TaskCount = p.Tasks.Count(t => !t.IsDeleted),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return projects;
    }
}
