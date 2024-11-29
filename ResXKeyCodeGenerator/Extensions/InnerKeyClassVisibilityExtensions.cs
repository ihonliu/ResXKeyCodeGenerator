namespace Ihon.ResXKeyCodeGenerator;

public static class InnerKeyClassVisibilityExtensions
{
    /// <summary>
    ///     Get the visibility keyword for the inner key class.
    /// </summary>
    /// <param name="visibility"></param>
    /// <param name="isOuterClassPublic"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string? GetVisibilityKeyword(this InnerKeyClassVisibility visibility, bool isOuterClassPublic = false)
    {
        return visibility switch
        {
            InnerKeyClassVisibility.NotGenerated => null,
            InnerKeyClassVisibility.Public => "public",
            InnerKeyClassVisibility.Internal => "internal",
            InnerKeyClassVisibility.Protected => "protected",
            InnerKeyClassVisibility.SameAsOuter => isOuterClassPublic ? "public" : "internal",
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };
    }
}
