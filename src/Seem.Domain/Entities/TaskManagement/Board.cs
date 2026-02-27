using Seem.Domain.Common;
using Seem.Domain.Exceptions;

namespace Seem.Domain.Entities.TaskManagement;

public class Board : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    private readonly List<BoardColumn> _columns = new();
    public IReadOnlyCollection<BoardColumn> Columns => _columns.AsReadOnly();

    private Board() { }

    internal static Board Create(string name, Guid projectId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Board name is required.");

        return new Board { Name = name, ProjectId = projectId };
    }

    public BoardColumn AddColumn(string name, int position, Enums.TaskItemStatus? mappedStatus = null, int? wipLimit = null)
    {
        var column = new BoardColumn
        {
            Name = name,
            Position = position,
            BoardId = Id,
            MappedStatus = mappedStatus,
            WipLimit = wipLimit
        };
        _columns.Add(column);
        return column;
    }

    public void Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Board name is required.");
        Name = name;
    }
}
