namespace Ihon.ResXKeyCodeGenerator;

// ReSharper disable once UnusedMember.Global
internal static class StringExtensions
{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static string? NullIfEmpty(this string? value)
    {
        return value.IsNullOrEmpty() ? null : value;
    }
}
