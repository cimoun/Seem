using MediatR;
using Seem.Application.Common.Interfaces;
using Seem.Domain.Exceptions;

namespace Seem.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.TaskItems.FindAsync([request.Id], cancellationToken)
            ?? throw new DomainException($"Task with ID '{request.Id}' not found.");

        if (task.IsDeleted)
            throw new DomainException($"Task with ID '{request.Id}' is already deleted.");

        task.IsDeleted = true;
        task.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
