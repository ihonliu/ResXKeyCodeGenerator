namespace Ihon.ResXKeyCodeGenerator;

public sealed record GlobalOptions // this must be a record or implement IEquatable<T>
{
    public GlobalOptions(AnalyzerConfigOptions options)
    {
        IsValid = true;

        if (!options.TryGetValue("build_property.MSBuildProjectFullPath", out var projectFullPath))
        {
            IsValid = false;
        }

        ProjectFullPath = projectFullPath!;

        if (options.TryGetValue("build_property.RootNamespace", out var rootNamespace))
        {
            RootNamespace = rootNamespace;
        }

        if (!options.TryGetValue("build_property.MSBuildProjectName", out var projectName))
        {
            IsValid = false;
        }

        ProjectName = projectName!;

        // Code from: https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#consume-msbuild-properties-and-metadata
        PublicClass =
            options.TryGetValue("build_property.ResXKeyCodeGenerator_PublicClass", out var publicClassSwitch) &&
            publicClassSwitch is { Length: > 0 } &&
            publicClassSwitch.Equals("true", StringComparison.OrdinalIgnoreCase);

        StaticClass =
            !(
                options.TryGetValue("build_property.ResXKeyCodeGenerator_StaticClass", out var staticClassSwitch) &&
                staticClassSwitch is { Length: > 0 } &&
                staticClassSwitch.Equals("false", StringComparison.OrdinalIgnoreCase)
            );

        PartialClass =
            options.TryGetValue("build_property.ResXKeyCodeGenerator_PartialClass", out var partialClassSwitch) &&
            partialClassSwitch is { Length: > 0 } &&
            partialClassSwitch.Equals("true", StringComparison.OrdinalIgnoreCase);

        ClassNamePostfix = string.Empty;
        if (options.TryGetValue("build_property.ResXKeyCodeGenerator_ClassNamePostfix", out var classNamePostfixSwitch))
        {
            ClassNamePostfix = classNamePostfixSwitch;
        }

        InnerKeyClassVisibility = InnerKeyClassVisibility.NotGenerated;
        if (
            options.TryGetValue("build_property.ResXKeyCodeGenerator_InnerKeyClassVisibility",
                out var innerKeyClassVisibility) &&
            Enum.TryParse(innerKeyClassVisibility, true, out InnerKeyClassVisibility v)
        )
        {
            InnerKeyClassVisibility = v;
        }

        InnerKeyClassName = null;
        if (options.TryGetValue("build_property.ResXKeyCodeGenerator_InnerKeyClassName", out var innerKeyClassName))
        {
            InnerKeyClassName = innerKeyClassName;
        }
    }

    public InnerKeyClassVisibility InnerKeyClassVisibility { get; }
    public string? InnerKeyClassName { get; }
    public bool PartialClass { get; }
    public string? RootNamespace { get; }
    public string ProjectFullPath { get; }
    public string ProjectName { get; }
    public bool StaticClass { get; }
    public bool PublicClass { get; }
    public string ClassNamePostfix { get; }
    public bool IsValid { get; }

    public static GlobalOptions Select(AnalyzerConfigOptionsProvider provider, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return new GlobalOptions(provider.GlobalOptions);
    }
}
