using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seem.Application.Common.Interfaces;

namespace Seem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public ProjectsController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Key,
                p.IsArchived,
                p.CreatedAt,
                TaskCount = p.Tasks.Count()
            })
            .ToListAsync(cancellationToken);

        return Ok(new { data = projects });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Boards)
            .ThenInclude(b => b.Columns)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (project == null) return NotFound();

        return Ok(new { data = project });
    }
}
