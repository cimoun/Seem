using Seem.Domain.Exceptions;

namespace Seem.Domain.ValueObjects;

public record DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateRange(DateTime start, DateTime end)
    {
        if (end < start)
            throw new DomainException("End date must be after start date.");

        Start = start;
        End = end;
    }

    public TimeSpan Duration => End - Start;
    public bool Contains(DateTime date) => date >= Start && date <= End;
}
