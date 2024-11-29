using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;
using static System.Guid;

namespace Ihon.ResXKeyCodeGenerator.Tests;

public class GeneratorTests
{
    private const string Text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:import namespace=""http://www.w3.org/XML/1998/namespace"" />
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""metadata"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" use=""required"" type=""xsd:string"" />
              <xsd:attribute name=""type"" type=""xsd:string"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""assembly"">
            <xsd:complexType>
              <xsd:attribute name=""alias"" type=""xsd:string"" />
              <xsd:attribute name=""name"" type=""xsd:string"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>2.0</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name=""CreateDate"" xml:space=""preserve"">
    <value>Oldest</value>
  </data>
  <data name=""CreateDateDescending"" xml:space=""preserve"">
    <value>Newest</value>
  </data>
</root>";

    private const string TextWithUnsupportedChar = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:import namespace=""http://www.w3.org/XML/1998/namespace"" />
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""metadata"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" use=""required"" type=""xsd:string"" />
              <xsd:attribute name=""type"" type=""xsd:string"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""assembly"">
            <xsd:complexType>
              <xsd:attribute name=""alias"" type=""xsd:string"" />
              <xsd:attribute name=""name"" type=""xsd:string"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>2.0</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name=""CreateDate"" xml:space=""preserve"">
    <value>Oldest</value>
  </data>
  <data name=""CreateDateDescending"" xml:space=""preserve"">
    <value>Newest</value>
  </data>
  <data name=""Sys.Name"" xml:space=""preserve"">
    <value>SystemName</value>
  </data>
</root>";

    private static void GenerateInnerKeys(
        IGenerator generator,
        bool publicClass = true,
        bool staticClass = false,
        bool partial = false,
        string innerKeyClassName = "Keys",
        InnerKeyClassVisibility innerKeyClassVisibility = InnerKeyClassVisibility.SameAsOuter
    )
    {
        var visibility = innerKeyClassVisibility.GetVisibilityKeyword(publicClass);
        if (visibility == null)
        {
            throw new ArgumentException("Cannot set NotGenerated visibility");
        }

        var expected = $@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace Resources;

{(publicClass ? "public" : "internal")}{(partial ? " partial" : string.Empty)}{(staticClass ? " static" : string.Empty)} class ActivityEntrySortRuleNames
{{
    {visibility}{(partial ? " partial" : string.Empty)}{(staticClass ? " static" : string.Empty)} class {innerKeyClassName}
    {{

        /// <summary>
        /// Name of resource CreateDate.
        /// </summary>
        public const string CreateDate = nameof(CreateDate);

        /// <summary>
        /// Name of resource CreateDateDescending.
        /// </summary>
        public const string CreateDateDescending = nameof(CreateDateDescending);
    }}
}}
";
        var (_, sourceCode, errorsAndWarnings) = generator.Generate(
            new FileOptions
            {
                LocalNamespace = "Ihon.Web.App_GlobalResources",
                CustomToolNamespace = "Resources",
                ClassName = "ActivityEntrySortRuleNames",
                PublicClass = publicClass,
                GroupedFile = new GroupedAdditionalFile(
                    new AdditionalTextWithHash(new AdditionalTextStub("", Text), NewGuid()),
                    Array.Empty<AdditionalTextWithHash>()
                ),
                StaticClass = staticClass,
                PartialClass = partial,
                InnerKeyClassVisibility = innerKeyClassVisibility,
                InnerKeyClassName = innerKeyClassName
            }
        );
        errorsAndWarnings.Should().BeNullOrEmpty();
        sourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    private static void GenerateKeys(
        IGenerator generator,
        bool publicClass = true,
        bool staticClass = false,
        bool partial = false
    )
    {
        var expected = $@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace Resources;

{(publicClass ? "public" : "internal")}{(staticClass ? " static" : string.Empty)}{(partial ? " partial" : string.Empty)} class ActivityEntrySortRuleNames
{{

    /// <summary>
    /// Name of resource CreateDate.
    /// </summary>
    public const string CreateDate = nameof(CreateDate);

    /// <summary>
    /// Name of resource CreateDateDescending.
    /// </summary>
    public const string CreateDateDescending = nameof(CreateDateDescending);

    /// <summary>
    /// Name of resource Sys.Name.
    /// </summary>
    public const string Sys_Name = ""Sys.Name"";
}}
";
        var (_, sourceCode, errorsAndWarnings) = generator.Generate(
            new FileOptions
            {
                LocalNamespace = "Ihon.Web.App_GlobalResources",
                CustomToolNamespace = "Resources",
                ClassName = "ActivityEntrySortRuleNames",
                PublicClass = publicClass,
                GroupedFile = new GroupedAdditionalFile(
                    new AdditionalTextWithHash(new AdditionalTextStub("", TextWithUnsupportedChar), NewGuid()),
                    Array.Empty<AdditionalTextWithHash>()
                ),
                StaticClass = staticClass,
                PartialClass = partial
            }
        );
        errorsAndWarnings.Should().BeNullOrEmpty();
        sourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    [Fact]
    public void Generate_StringBuilder_InnerKey()
    {
        var generator = new StringBuilderGenerator();
        GenerateInnerKeys(generator);
        GenerateInnerKeys(generator, innerKeyClassVisibility: InnerKeyClassVisibility.Internal);
        GenerateInnerKeys(generator, innerKeyClassVisibility: InnerKeyClassVisibility.Internal,
            innerKeyClassName: "InnerKeys");
    }

    [Fact]
    public void Generate_StringBuilder_NewLine()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <!--
    Microsoft ResX Schema

    Version 2.0

    The primary goals of this format is to allow a simple XML format
    that is mostly human readable. The generation and parsing of the
    various data types are done through the TypeConverter classes
    associated with the data types.

    Example:

    ... ado.net/XML headers & schema ...
    <resheader name=""resmimetype"">text/microsoft-resx</resheader>
    <resheader name=""version"">2.0</resheader>
    <resheader name=""reader"">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name=""writer"">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name=""Name1""><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name=""Color1"" type=""System.Drawing.Color, System.Drawing"">Blue</data>
    <data name=""Bitmap1"" mimetype=""application/x-microsoft.net.object.binary.base64"">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name=""Icon1"" type=""System.Drawing.Icon, System.Drawing"" mimetype=""application/x-microsoft.net.object.bytearray.base64"">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>

    There are any number of ""resheader"" rows that contain simple
    name/value pairs.

    Each data row contains a name, and value. The row also contains a
    type or mimetype. Type corresponds to a .NET class that support
    text/value conversion through the TypeConverter architecture.
    Classes that don't support this are serialized and stored with the
    mimetype set.

    The mimetype is used for serialized objects, and tells the
    ResXResourceReader how to depersist the object. This is currently not
    extensible. For a given mimetype the value must be set accordingly:

    Note - application/x-microsoft.net.object.binary.base64 is the format
    that the ResXResourceWriter will generate, however the reader can
    read any of the formats listed below.

    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:import namespace=""http://www.w3.org/XML/1998/namespace"" />
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""metadata"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" use=""required"" type=""xsd:string"" />
              <xsd:attribute name=""type"" type=""xsd:string"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""assembly"">
            <xsd:complexType>
              <xsd:attribute name=""alias"" type=""xsd:string"" />
              <xsd:attribute name=""name"" type=""xsd:string"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>2.0</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name=""EntryDeleted"" xml:space=""preserve"">
    <value>This entry has been deleted. It is still temporarily accessible, but won't show up in any of the listings.</value>
  </data>
  <data name=""EntryMergedTo"" xml:space=""preserve"">
    <value>This entry was merged to</value>
  </data>
  <data name=""EntryStatusExplanation"" xml:space=""preserve"">
    <value>Draft = entry is missing crucial information. This status indicates that you're requesting additional information to be added or corrected.&lt;br /&gt;
Finished = The entry has all the necessary information, but it hasn't been inspected by a trusted user yet.&lt;br /&gt;
Approved = The entry has been inspected and approved by a trusted user. Approved entries can only be edited by trusted users.</value>
  </data>
  <data name=""Locked"" xml:space=""preserve"">
    <value>This entry is locked, meaning that only moderators are allowed to edit it.</value>
  </data>
  <data name=""NameLanguageHelp"" xml:space=""preserve"">
    <value>Choose the language for this name. ""Original"" is the name in original language that isn't English, for example Japanese. If the original language is English, do not input a name in the ""Original"" language.</value>
  </data>
  <data name=""RevisionHidden"" xml:space=""preserve"">
    <value>This page revision has been hidden.</value>
  </data>
</root>";
        var expected = @"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace Ihon.Web.App_GlobalResources;

public static class CommonMessages
{

    /// <summary>
    /// Name of resource EntryDeleted.
    /// </summary>
    public const string EntryDeleted = nameof(EntryDeleted);

    /// <summary>
    /// Name of resource EntryMergedTo.
    /// </summary>
    public const string EntryMergedTo = nameof(EntryMergedTo);

    /// <summary>
    /// Name of resource EntryStatusExplanation.
    /// </summary>
    public const string EntryStatusExplanation = nameof(EntryStatusExplanation);

    /// <summary>
    /// Name of resource Locked.
    /// </summary>
    public const string Locked = nameof(Locked);

    /// <summary>
    /// Name of resource NameLanguageHelp.
    /// </summary>
    public const string NameLanguageHelp = nameof(NameLanguageHelp);

    /// <summary>
    /// Name of resource RevisionHidden.
    /// </summary>
    public const string RevisionHidden = nameof(RevisionHidden);
}
";
        var generator = new StringBuilderGenerator();
        var (_, sourceCode, errorsAndWarnings) = generator.Generate(
            new FileOptions
            {
                LocalNamespace = "Ihon.Web.App_GlobalResources",
                CustomToolNamespace = null,
                ClassName = "CommonMessages",
                GroupedFile = new GroupedAdditionalFile(
                    new AdditionalTextWithHash(new AdditionalTextStub("", text), NewGuid()),
                    Array.Empty<AdditionalTextWithHash>()
                ),
                PublicClass = true,
                StaticClass = true
            }
        );
        errorsAndWarnings.Should().BeNullOrEmpty();
        sourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    [Fact]
    public void Generate_StringBuilder()
    {
        var generator = new StringBuilderGenerator();
        GenerateKeys(generator);
        GenerateKeys(generator, false);
    }

    [Fact]
    public void Generate_StringBuilder_Name_MemberSameAsFileGivesWarning()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <data name=""CommonMessages"" xml:space=""preserve"">
    <value>Works.</value>
  </data>
</root>";

        var generator = new StringBuilderGenerator();
        var (_, _, errorsAndWarnings) = generator.Generate(
            new FileOptions
            {
                LocalNamespace = "Ihon.Web.App_GlobalResources",
                GroupedFile = new GroupedAdditionalFile(
                    new AdditionalTextWithHash(new AdditionalTextStub("", text), NewGuid()),
                    Array.Empty<AdditionalTextWithHash>()
                ),
                CustomToolNamespace = null,
                ClassName = "CommonMessages",
                PublicClass = true,
                StaticClass = true
            }
        );
        var errs = errorsAndWarnings.ToList();
        errs.Should().NotBeNull();
        errs.Should().HaveCount(1);
        errs[0].Id.Should().Be("ResXKeyCodeGenerator002");
        errs[0].Severity.Should().Be(DiagnosticSeverity.Warning);
        errs[0].GetMessage().Should().Contain("CommonMessages");
        errs[0].Location.GetLineSpan().StartLinePosition.Line.Should().Be(2);
    }

    [Fact]
    public void GetLocalNamespace_ShouldNotGenerateIllegalNamespace()
    {
        var ns = Utilities.GetLocalNamespace("resx", "asd.asd", "path", "name", "root");
        ns.Should().Be("root");
    }

    [Fact]
    public void ResxFileName_ShouldNotGenerateIllegalClassNames()
    {
        var ns = Utilities.GetClassNameFromPath("test.cshtml.resx");
        ns.Should().Be("test");
    }
}
