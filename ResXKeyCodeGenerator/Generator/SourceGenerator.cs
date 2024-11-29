namespace Ihon.ResXKeyCodeGenerator;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
    private static readonly IGenerator Generator = new StringBuilderGenerator();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var globalOptions = context.AnalyzerConfigOptionsProvider.Select(GlobalOptions.Select);

        // Note: Each Resx file will get a hash (random guid) so we can easily differentiate in the pipeline when the file changed or just some options
        var allResxFiles = context.AdditionalTextsProvider.Where(static af => af.Path.EndsWith(".resx"))
            .Select(static (f, _) => new AdditionalTextWithHash(f, Guid.NewGuid()));

        var monitor = allResxFiles.Collect().SelectMany(static (x, _) => GroupResxFiles.Group(x));

        var inputs = monitor
            .Combine(globalOptions)
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select(static (x, _) => FileOptions.Select(
                x.Left.Left,
                x.Right,
                x.Left.Right
            ))
            .Where(static x => x.IsValid);

        context.RegisterSourceOutput(inputs, (ctx, file) =>
        {
            var (generatedFileName, sourceCode, errorsAndWarnings) =
                Generator.Generate(file, ctx.CancellationToken);
            foreach (var sourceErrorsAndWarning in errorsAndWarnings)
            {
                ctx.ReportDiagnostic(sourceErrorsAndWarning);
            }

            ctx.AddSource(generatedFileName, sourceCode);
        });
    }
}
