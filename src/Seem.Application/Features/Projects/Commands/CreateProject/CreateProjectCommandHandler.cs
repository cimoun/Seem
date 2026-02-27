using MediatR;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;
using Seem.Application.Features.Projects.DTOs;
using Seem.Domain.Entities.TaskManagement;
using Seem.Domain.Enums;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDetailDto>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDetailDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var existingProject = await _context.Projects
            .AnyAsync(p => p.Key == request.Key.ToUpperInvariant(), cancellationToken);

        if (existingProject)
            throw new DomainException($"A project with key '{request.Key.ToUpperInvariant()}' already exists.");

        var project = Project.Create(request.Name, request.Key, request.Description, request.Color);

        var board = project.AddBoard("Main Board");
        board.AddColumn("To Do", 0, TaskItemStatus.Todo);
        board.AddColumn("In Progress", 1, TaskItemStatus.InProgress);
        board.AddColumn("In Review", 2, TaskItemStatus.InReview);
        board.AddColumn("Done", 3, TaskItemStatus.Done);

        await _context.Projects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new ProjectDetailDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Key = project.Key,
            Color = project.Color.HexValue,
            IsArchived = project.IsArchived,
            TaskCount = 0,
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
