using MediatR;
using Seem.Application.Common.Interfaces;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Projects.Commands.ArchiveProject;

public class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ArchiveProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync([request.Id], cancellationToken)
            ?? throw new DomainException($"Project with ID '{request.Id}' not found.");

        project.Archive();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
