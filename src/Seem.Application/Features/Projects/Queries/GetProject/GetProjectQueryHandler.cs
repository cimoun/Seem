using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Projects.DTOs;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Projects.Queries.GetProject;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetProjectQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDetailDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Boards)
                .ThenInclude(b => b.Columns)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new DomainException($"Project with ID '{request.Id}' not found.");

        var taskCount = await _context.TaskItems
            .CountAsync(t => t.ProjectId == request.Id && !t.IsDeleted, cancellationToken);

        return new ProjectDetailDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Key = project.Key,
            Color = project.Color.HexValue,
            IsArchived = project.IsArchived,
            TaskCount = taskCount,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Boards = project.Boards.Select(b => new BoardDto
            {
                Id = b.Id,
                Name = b.Name,
                ProjectId = b.ProjectId,
                Columns = b.Columns.Select(c => new BoardColumnDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Position = c.Position,
                    MappedStatus = c.MappedStatus,
                    WipLimit = c.WipLimit
                }).OrderBy(c => c.Position).ToList()
            }).ToList()
        };
    }
}
