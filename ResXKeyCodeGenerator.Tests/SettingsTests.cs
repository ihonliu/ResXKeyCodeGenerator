using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Ihon.ResXKeyCodeGenerator.Tests;

public class SettingsTests
{
    private static readonly GlobalOptions LocalGlobalOptionsForTest = GlobalOptions.Select(
        new AnalyzerConfigOptionsProviderStub(
            new AnalyzerConfigOptionsStub
            {
                RootNamespace = "namespace1",
                MSBuildProjectFullPath = "project1.csproj",
                MSBuildProjectName = "project1"
            },
            null!
        ),
        default
    );

    [Fact]
    public void GlobalDefaults()
    {
        var globalOptions = LocalGlobalOptionsForTest;
        globalOptions.ProjectName.Should().Be("project1");
        globalOptions.RootNamespace.Should().Be("namespace1");
        globalOptions.ProjectFullPath.Should().Be("project1.csproj");
        globalOptions.ClassNamePostfix.Should().BeNullOrEmpty();
        globalOptions.InnerKeyClassVisibility.Should().Be(InnerKeyClassVisibility.NotGenerated);
        globalOptions.StaticClass.Should().Be(true);
        globalOptions.PublicClass.Should().Be(false);
        globalOptions.PartialClass.Should().Be(false);
        globalOptions.IsValid.Should().Be(true);
        globalOptions.InnerKeyClassName.Should().BeNull();
    }

    [Fact]
    public void GlobalSettings_CanReadAll()
    {
        var globalOptions = GlobalOptions.Select(
            new AnalyzerConfigOptionsProviderStub(
                new AnalyzerConfigOptionsStub
                {
                    RootNamespace = "namespace1",
                    MSBuildProjectFullPath = "project1.csproj",
                    MSBuildProjectName = "project1",
                    ResXKeyCodeGenerator_ClassNamePostfix = "test3",
                    ResXKeyCodeGenerator_StaticClass = "false",
                    ResXKeyCodeGenerator_PublicClass = "true",
                    ResXKeyCodeGenerator_PartialClass = "true",
                    ResXKeyCodeGenerator_InnerKeyClassVisibility = "Public",
                    ResXKeyCodeGenerator_InnerKeyClassName = "test3"
                },
                null!
            ),
            default
        );
        globalOptions.RootNamespace.Should().Be("namespace1");
        globalOptions.ProjectFullPath.Should().Be("project1.csproj");
        globalOptions.ProjectName.Should().Be("project1");
        globalOptions.ClassNamePostfix.Should().Be("test3");
        globalOptions.InnerKeyClassVisibility.Should().Be(InnerKeyClassVisibility.Public);
        globalOptions.StaticClass.Should().Be(false);
        globalOptions.PublicClass.Should().Be(true);
        globalOptions.PartialClass.Should().Be(true);
        globalOptions.IsValid.Should().Be(true);
        globalOptions.InnerKeyClassName.Should().Be("test3");
    }

    [Fact]
    public void FileDefaults()
    {
        var fileOptions = FileOptions.Select(
            new GroupedAdditionalFile(
                new AdditionalTextWithHash(new AdditionalTextStub("Path1.resx"), Guid.NewGuid()),
                Array.Empty<AdditionalTextWithHash>()
            ),
            new AnalyzerConfigOptionsProviderStub(
                null!,
                new AnalyzerConfigOptionsStub()
            ),
            LocalGlobalOptionsForTest
        );
        fileOptions.StaticClass.Should().Be(true);
        fileOptions.PublicClass.Should().Be(false);
        fileOptions.PartialClass.Should().Be(false);
        fileOptions.LocalNamespace.Should().Be("namespace1");
        fileOptions.CustomToolNamespace.Should().BeNullOrEmpty();
        fileOptions.GroupedFile.MainFile.File.Path.Should().Be("Path1.resx");
        fileOptions.ClassName.Should().Be("Path1");
        fileOptions.IsValid.Should().Be(true);
        fileOptions.InnerKeyClassVisibility.Should().Be(InnerKeyClassVisibility.NotGenerated);
        fileOptions.InnerKeyClassName.Should().Be(null);
    }

    [Theory]
    [InlineData("project1.csproj", "Path1.resx", "", "project1")]
    [InlineData("project1.csproj", "Path1.resx", "rootNamespace", "rootNamespace")]
    [InlineData(@"ProjectFolder\project1.csproj", @"ProjectFolder\SubFolder\Path1.resx", "rootNamespace",
        "rootNamespace.SubFolder")]
    [InlineData(@"ProjectFolder\project1.csproj", @"ProjectFolder\SubFolder With Space\Path1.resx", "rootNamespace",
        "rootNamespace.SubFolder_With_Space")]
    [InlineData(@"ProjectFolder\project1.csproj", @"ProjectFolder\SubFolder\Path1.resx", "", "SubFolder")]
    [InlineData(@"ProjectFolder\8 project.csproj", @"ProjectFolder\Path1.resx", "", "_8_project")]
    [InlineData(@"ProjectFolder\8 project.csproj", @"ProjectFolder\SubFolder\Path1.resx", "", "SubFolder")]
    public void FileSettings_RespectsEmptyRootNamespace(
        string msBuildProjectFullPath,
        string mainFile,
        string rootNamespace,
        string expectedLocalNamespace
    )
    {
        msBuildProjectFullPath = msBuildProjectFullPath.Replace('\\', Path.DirectorySeparatorChar);
        mainFile = mainFile.Replace('\\', Path.DirectorySeparatorChar);
        var fileOptions = FileOptions.Select(
            new GroupedAdditionalFile(
                new AdditionalTextWithHash(new AdditionalTextStub(mainFile), Guid.NewGuid()),
                Array.Empty<AdditionalTextWithHash>()
            ),
            new AnalyzerConfigOptionsProviderStub(
                null!,
                new AnalyzerConfigOptionsStub()
            ),
            GlobalOptions.Select(
                new AnalyzerConfigOptionsProviderStub(
                    new AnalyzerConfigOptionsStub
                    {
                        MSBuildProjectName = Path.GetFileNameWithoutExtension(msBuildProjectFullPath),
                        RootNamespace = rootNamespace,
                        MSBuildProjectFullPath = msBuildProjectFullPath
                    },
                    null!
                ),
                default
            )
        );
        fileOptions.StaticClass.Should().Be(true);
        fileOptions.PublicClass.Should().Be(false);
        fileOptions.PartialClass.Should().Be(false);
        fileOptions.LocalNamespace.Should().Be(expectedLocalNamespace);
        fileOptions.CustomToolNamespace.Should().BeNullOrEmpty();
        fileOptions.GroupedFile.MainFile.File.Path.Should().Be(mainFile);
        fileOptions.ClassName.Should().Be("Path1");
        fileOptions.IsValid.Should().Be(true);
    }

    [Fact]
    public void File_PostFix()
    {
        var fileOptions = FileOptions.Select(
            new GroupedAdditionalFile(
                new AdditionalTextWithHash(new AdditionalTextStub("Path1.resx"), Guid.NewGuid()),
                Array.Empty<AdditionalTextWithHash>()
            ),
            new AnalyzerConfigOptionsProviderStub(
                null!,
                new AnalyzerConfigOptionsStub { ClassNamePostfix = "test1" }
            ),
            LocalGlobalOptionsForTest
        );
        fileOptions.ClassName.Should().Be("Path1test1");
        fileOptions.IsValid.Should().Be(true);
    }

    [Fact]
    public void FileSettings_CanReadAll()
    {
        var fileOptions = FileOptions.Select(
            new GroupedAdditionalFile(
                new AdditionalTextWithHash(new AdditionalTextStub("Path1.resx"), Guid.NewGuid()),
                Array.Empty<AdditionalTextWithHash>()
            ),
            new AnalyzerConfigOptionsProviderStub(
                null!,
                new AnalyzerConfigOptionsStub
                {
                    RootNamespace = "namespace1",
                    MSBuildProjectFullPath = "project1.csproj",
                    CustomToolNamespace = "ns1",
                    ResXKeyCodeGenerator_InnerKeyClassVisibility = "public",
                    StaticClass = "false",
                    PublicClass = "true",
                    PartialClass = "true",
                    InnerKeyClassVisibility = "public",
                    InnerKeyClassName = "test1"
                }
            ),
            LocalGlobalOptionsForTest
        );
        fileOptions.StaticClass.Should().Be(false);
        fileOptions.PublicClass.Should().Be(true);
        fileOptions.PartialClass.Should().Be(true);
        fileOptions.IsValid.Should().Be(true);
        fileOptions.LocalNamespace.Should().Be("namespace1");
        fileOptions.CustomToolNamespace.Should().Be("ns1");
        fileOptions.GroupedFile.MainFile.File.Path.Should().Be("Path1.resx");
        fileOptions.ClassName.Should().Be("Path1");
        fileOptions.InnerKeyClassVisibility.Should().Be(InnerKeyClassVisibility.Public);
        fileOptions.InnerKeyClassName.Should().Be("test1");
    }

    [Fact]
    public void FileSettings_RespectsGlobalDefaults()
    {
        var globalOptions = GlobalOptions.Select(
            new AnalyzerConfigOptionsProviderStub(
                new AnalyzerConfigOptionsStub
                {
                    RootNamespace = "namespace1",
                    MSBuildProjectFullPath = "project1.csproj",
                    MSBuildProjectName = "project1",
                    ResXKeyCodeGenerator_ClassNamePostfix = "test3",
                    ResXKeyCodeGenerator_StaticClass = "false",
                    ResXKeyCodeGenerator_PublicClass = "true",
                    ResXKeyCodeGenerator_PartialClass = "true"
                },
                null!
            ),
            default
        );
        var fileOptions = FileOptions.Select(
            new GroupedAdditionalFile(
                new AdditionalTextWithHash(new AdditionalTextStub("Path1.resx"), Guid.NewGuid()),
                Array.Empty<AdditionalTextWithHash>()
            ),
            new AnalyzerConfigOptionsProviderStub(
                null!,
                new AnalyzerConfigOptionsStub()
            ),
            globalOptions
        );
        fileOptions.StaticClass.Should().Be(false);
        fileOptions.PublicClass.Should().Be(true);
        fileOptions.PartialClass.Should().Be(true);
        fileOptions.IsValid.Should().Be(true);
        fileOptions.LocalNamespace.Should().Be("namespace1");
        fileOptions.CustomToolNamespace.Should().BeNullOrEmpty();
        fileOptions.GroupedFile.MainFile.File.Path.Should().Be("Path1.resx");
        fileOptions.ClassName.Should().Be("Path1test3");
        fileOptions.IsValid.Should().Be(true);
    }


    private class AnalyzerConfigOptionsStub : AnalyzerConfigOptions
    {
        // ReSharper disable InconsistentNaming
        public string? MSBuildProjectFullPath { get; init; }

        // ReSharper disable InconsistentNaming
        public string? MSBuildProjectName { get; init; }
        public string? RootNamespace { get; init; }
        public string? ResXKeyCodeGenerator_ClassNamePostfix { get; init; }
        public string? ResXKeyCodeGenerator_PublicClass { get; init; }
        public string? ResXKeyCodeGenerator_StaticClass { get; init; }
        public string? ResXKeyCodeGenerator_PartialClass { get; init; }
        public string? ResXKeyCodeGenerator_InnerKeyClassVisibility { get; init; }
        public string? CustomToolNamespace { get; init; }
        public string? ClassNamePostfix { get; init; }
        public string? PublicClass { get; init; }
        public string? StaticClass { get; init; }
        public string? PartialClass { get; init; }
        public string? InnerKeyClassVisibility { get; init; }
        public string? InnerKeyClassName { get; init; }
        public string? ResXKeyCodeGenerator_InnerKeyClassName { get; init; }

        // ReSharper restore InconsistentNaming

        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
        {
            string? GetVal()
            {
                return key switch
                {
                    "build_property.MSBuildProjectFullPath" => MSBuildProjectFullPath,
                    "build_property.MSBuildProjectName" => MSBuildProjectName,
                    "build_property.RootNamespace" => RootNamespace,
                    "build_property.ResXKeyCodeGenerator_ClassNamePostfix" => ResXKeyCodeGenerator_ClassNamePostfix,
                    "build_property.ResXKeyCodeGenerator_PublicClass" => ResXKeyCodeGenerator_PublicClass,
                    "build_property.ResXKeyCodeGenerator_StaticClass" => ResXKeyCodeGenerator_StaticClass,
                    "build_property.ResXKeyCodeGenerator_PartialClass" => ResXKeyCodeGenerator_PartialClass,
                    "build_property.ResXKeyCodeGenerator_InnerKeyClassVisibility" =>
                        ResXKeyCodeGenerator_InnerKeyClassVisibility,
                    "build_property.ResXKeyCodeGenerator_InnerKeyClassName" => ResXKeyCodeGenerator_InnerKeyClassName,
                    "build_metadata.EmbeddedResource.CustomToolNamespace" => CustomToolNamespace,
                    "build_metadata.EmbeddedResource.ClassNamePostfix" => ClassNamePostfix,
                    "build_metadata.EmbeddedResource.PublicClass" => PublicClass,
                    "build_metadata.EmbeddedResource.StaticClass" => StaticClass,
                    "build_metadata.EmbeddedResource.PartialClass" => PartialClass,
                    "build_metadata.EmbeddedResource.InnerKeyClassVisibility" => InnerKeyClassVisibility,
                    "build_metadata.EmbeddedResource.InnerKeyClassName" => InnerKeyClassName,
                    _ => null
                };
            }

            value = GetVal();
            return value is not null;
        }
    }

    private class AnalyzerConfigOptionsProviderStub(
        AnalyzerConfigOptions globalOptions,
        AnalyzerConfigOptions fileOptions)
        : AnalyzerConfigOptionsProvider
    {
        public override AnalyzerConfigOptions GlobalOptions { get; } = globalOptions;

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
        {
            throw new NotImplementedException();
        }

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        {
            return fileOptions;
        }
    }
}
