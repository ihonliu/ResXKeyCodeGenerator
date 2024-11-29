namespace Ihon.ResXKeyCodeGenerator;

internal static class StringBuilderExtensions
{
    public static void AppendLineLf(this StringBuilder builder)
    {
        builder.Append('\n');
    }

    public static void AppendLineLf(this StringBuilder builder, string value)
    {
        builder.Append(value);
        builder.AppendLineLf();
    }
}
