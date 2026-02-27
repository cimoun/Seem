using System.Text.RegularExpressions;
using Seem.Domain.Exceptions;

namespace Seem.Domain.ValueObjects;

public partial record Color
{
    public string HexValue { get; }

    public Color(string hex)
    {
        if (!HexColorRegex().IsMatch(hex))
            throw new DomainException($"Invalid hex color: {hex}");
        HexValue = hex;
    }

    public static implicit operator string(Color color) => color.HexValue;
    public static implicit operator Color(string hex) => new(hex);

    [GeneratedRegex(@"^#[0-9A-Fa-f]{6}$")]
    private static partial Regex HexColorRegex();
}
