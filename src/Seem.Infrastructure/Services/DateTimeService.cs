using Seem.Application.Common.Interfaces;

namespace Seem.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
