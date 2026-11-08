using System.Diagnostics.CodeAnalysis;

namespace Facturity.Modules.SharedKernel.Domain;

public static class Validate
{
    public static bool IsEmpty([NotNullWhen(false)] string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool ExceedsMaximumLength(string? value, int maximumLength)
    {
        return !string.IsNullOrWhiteSpace(value) && value.Length > maximumLength;
    }

    public static bool DoesNotMeetMinimumLength(string? value, int minimumLength)
    {
        return !string.IsNullOrWhiteSpace(value) && value.Length < minimumLength;
    }

    public static bool NotExactLength(string? value, int exactLength)
    {
        return !string.IsNullOrWhiteSpace(value) && value.Length != exactLength;
    }
}
