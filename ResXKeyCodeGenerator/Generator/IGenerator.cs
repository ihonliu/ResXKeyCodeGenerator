namespace Ihon.ResXKeyCodeGenerator;

public interface IGenerator
{
    /// <summary>
    ///     Generate source file with properties for each translated resource
    /// </summary>
    (string GeneratedFileName, string SourceCode, IEnumerable<Diagnostic> ErrorsAndWarnings)
        Generate(FileOptions options, CancellationToken cancellationToken = default);
}
