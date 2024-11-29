namespace Ihon.ResXKeyCodeGenerator;

public readonly record struct FileOptions
{
    public FileOptions(
        GroupedAdditionalFile groupedFile,
        AnalyzerConfigOptions options,
        GlobalOptions globalOptions
    )
    {
        GroupedFile = groupedFile;
        var resxFilePath = groupedFile.MainFile.File.Path;

        var classNameFromFileName = Utilities.GetClassNameFromPath(resxFilePath);

        var detectedNamespace = Utilities.GetLocalNamespace(
            resxFilePath,
            options.TryGetValue("build_metadata.EmbeddedResource.Link", out var link) &&
            link is { Length: > 0 }
                ? link
                : null,
            globalOptions.ProjectFullPath,
            globalOptions.ProjectName,
            globalOptions.RootNamespace);

        LocalNamespace =
            options.TryGetValue("build_metadata.EmbeddedResource.TargetPath", out var targetPath) &&
            targetPath is { Length: > 0 }
                ? Utilities.GetLocalNamespace(
                    resxFilePath, targetPath,
                    globalOptions.ProjectFullPath,
                    globalOptions.ProjectName,
                    globalOptions.RootNamespace)
                : string.IsNullOrEmpty(detectedNamespace)
                    ? Utilities.SanitizeNamespace(globalOptions.ProjectName)
                    : detectedNamespace;

        CustomToolNamespace =
            options.TryGetValue("build_metadata.EmbeddedResource.CustomToolNamespace", out var customToolNamespace) &&
            customToolNamespace is { Length: > 0 }
                ? customToolNamespace
                : null;

        ClassName =
            options.TryGetValue("build_metadata.EmbeddedResource.ClassNamePostfix", out var perFileClassNameSwitch) &&
            perFileClassNameSwitch is { Length: > 0 }
                ? classNameFromFileName + perFileClassNameSwitch
                : classNameFromFileName + globalOptions.ClassNamePostfix;

        PublicClass =
            options.TryGetValue("build_metadata.EmbeddedResource.PublicClass", out var perFilePublicClassSwitch) &&
            perFilePublicClassSwitch is { Length: > 0 }
                ? perFilePublicClassSwitch.Equals("true", StringComparison.OrdinalIgnoreCase)
                : globalOptions.PublicClass;

        StaticClass =
            options.TryGetValue("build_metadata.EmbeddedResource.StaticClass", out var perFileStaticClassSwitch) &&
            perFileStaticClassSwitch is { Length: > 0 }
                ? !perFileStaticClassSwitch.Equals("false", StringComparison.OrdinalIgnoreCase)
                : globalOptions.StaticClass;

        PartialClass =
            options.TryGetValue("build_metadata.EmbeddedResource.PartialClass", out var partialClassSwitch) &&
            partialClassSwitch is { Length: > 0 }
                ? partialClassSwitch.Equals("true", StringComparison.OrdinalIgnoreCase)
                : globalOptions.PartialClass;

        InnerKeyClassVisibility =
            options.TryGetValue("build_metadata.EmbeddedResource.InnerKeyClassVisibility",
                out var innerKeyClassVisibilitySwitch) &&
            Enum.TryParse(innerKeyClassVisibilitySwitch, true, out InnerKeyClassVisibility v)
                ? v
                : globalOptions.InnerKeyClassVisibility;

        InnerKeyClassName =
            options.TryGetValue("build_metadata.EmbeddedResource.InnerKeyClassName", out var innerKeyClassName) &&
            innerKeyClassName is { Length: > 0 }
                ? innerKeyClassName
                : globalOptions.InnerKeyClassName;

        IsValid = globalOptions.IsValid;
    }

    public bool PartialClass { get; init; }
    public GroupedAdditionalFile GroupedFile { get; init; }
    public bool StaticClass { get; init; }
    public bool PublicClass { get; init; }
    public string ClassName { get; init; }
    public string? CustomToolNamespace { get; init; }
    public string LocalNamespace { get; init; }
    public bool IsValid { get; init; }
    public InnerKeyClassVisibility InnerKeyClassVisibility { get; init; }
    public string? InnerKeyClassName { get; init; }

    public static FileOptions Select(
        GroupedAdditionalFile file,
        AnalyzerConfigOptionsProvider options,
        GlobalOptions globalOptions
    )
    {
        return new FileOptions(
            file,
            options.GetOptions(file.MainFile.File),
            globalOptions
        );
    }
}
