using MediatR;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Projects.DTOs;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync([request.Id], cancellationToken)
            ?? throw new DomainException($"Project with ID '{request.Id}' not found.");

        project.Update(request.Name, request.Description);

        await _context.SaveChangesAsync(cancellationToken);

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Key = project.Key,
            Color = project.Color.HexValue,
            IsArchived = project.IsArchived,
            TaskCount = 0,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }
}
