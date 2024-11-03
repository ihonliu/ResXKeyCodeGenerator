﻿using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;
using static System.Guid;

namespace Catglobe.ResXFileCodeGenerator.Tests;

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


    private static void Generate(
        IGenerator generator,
        bool publicClass = true,
        bool staticClass = true,
        bool partial = false,
        bool nullForgivingOperators = false,
        bool staticMembers = true
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
using System.Globalization;
using System.Resources;

{(publicClass ? "public" : "internal")}{(staticClass ? " static" : string.Empty)}{(partial ? " partial" : string.Empty)} class ActivityEntrySortRuleNames
{{
    private static ResourceManager? s_resourceManager;
    public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager(""Catglobe.Web.App_GlobalResources.ActivityEntrySortRuleNames"", typeof(ActivityEntrySortRuleNames).Assembly);
    public{(staticMembers ? " static" : string.Empty)} CultureInfo? CultureInfo {{ get; set; }}

    /// <summary>
    /// Looks up a localized string similar to Oldest.
    /// </summary>
    public{(staticMembers ? " static" : string.Empty)} string{(nullForgivingOperators ? string.Empty : "?")} CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo){(nullForgivingOperators ? "!" : string.Empty)};

    /// <summary>
    /// Looks up a localized string similar to Newest.
    /// </summary>
    public{(staticMembers ? " static" : string.Empty)} string{(nullForgivingOperators ? string.Empty : "?")} CreateDateDescending => ResourceManager.GetString(nameof(CreateDateDescending), CultureInfo){(nullForgivingOperators ? "!" : string.Empty)};
}}
";
        var (_, SourceCode, ErrorsAndWarnings) = generator.Generate(
            options: new FileOptions()
            {
                LocalNamespace = "Catglobe.Web.App_GlobalResources",
                EmbeddedFilename = "Catglobe.Web.App_GlobalResources.ActivityEntrySortRuleNames",
                CustomToolNamespace = "Resources",
                ClassName = "ActivityEntrySortRuleNames",
                GroupedFile = new GroupedAdditionalFile(
                    mainFile: new AdditionalTextWithHash(new AdditionalTextStub("", Text), NewGuid()),
                    subFiles: Array.Empty<AdditionalTextWithHash>()
                ),
                PublicClass = publicClass,
                NullForgivingOperators = nullForgivingOperators,
                StaticClass = staticClass,
                PartialClass = partial,
                StaticMembers = staticMembers
            }
        );
        ErrorsAndWarnings.Should().BeNullOrEmpty();
        SourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    private static void GenerateInner(
        IGenerator generator,
        bool publicClass = true,
        bool staticClass = false,
        bool partial = false,
        bool nullForgivingOperators = false,
        bool staticMembers = true,
        string innerClassName = "inner",
        InnerClassVisibility innerClassVisibility = InnerClassVisibility.SameAsOuter,
        string innerClassInstanceName = ""
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
using System.Globalization;
using System.Resources;

{(publicClass ? "public" : "internal")}{(partial ? " partial" : string.Empty)}{(staticClass ? " static" : string.Empty)} class ActivityEntrySortRuleNames
{{{(string.IsNullOrEmpty(innerClassInstanceName) ? string.Empty : $"\n    public {innerClassName} {innerClassInstanceName} {{ get; }} = new();\n")}
    {(publicClass ? "public" : "internal")}{(partial ? " partial" : string.Empty)}{(staticClass ? " static" : string.Empty)} class {innerClassName}
    {{
        private static ResourceManager? s_resourceManager;
        public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager(""Catglobe.Web.App_GlobalResources.ActivityEntrySortRuleNames"", typeof({innerClassName}).Assembly);
        public{(staticMembers ? " static" : string.Empty)} CultureInfo? CultureInfo {{ get; set; }}

        /// <summary>
        /// Looks up a localized string similar to Oldest.
        /// </summary>
        public{(staticMembers ? " static" : string.Empty)} string{(nullForgivingOperators ? string.Empty : "?")} CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo){(nullForgivingOperators ? "!" : string.Empty)};

        /// <summary>
        /// Looks up a localized string similar to Newest.
        /// </summary>
        public{(staticMembers ? " static" : string.Empty)} string{(nullForgivingOperators ? string.Empty : "?")} CreateDateDescending => ResourceManager.GetString(nameof(CreateDateDescending), CultureInfo){(nullForgivingOperators ? "!" : string.Empty)};
    }}
}}
";
        var (_, SourceCode, ErrorsAndWarnings) = generator.Generate(
            options: new FileOptions()
            {
                LocalNamespace = "Catglobe.Web.App_GlobalResources",
                EmbeddedFilename = "Catglobe.Web.App_GlobalResources.ActivityEntrySortRuleNames",
                CustomToolNamespace = "Resources",
                ClassName = "ActivityEntrySortRuleNames",
                PublicClass = publicClass,
                NullForgivingOperators = nullForgivingOperators,
                GroupedFile = new GroupedAdditionalFile(
                    mainFile: new AdditionalTextWithHash(new AdditionalTextStub("", Text), NewGuid()),
                    subFiles: Array.Empty<AdditionalTextWithHash>()
                ),
                StaticClass = staticClass,
                PartialClass = partial,
                StaticMembers = staticMembers,
                InnerClassName = innerClassName,
                InnerClassVisibility = innerClassVisibility,
                InnerClassInstanceName = innerClassInstanceName
			}
        );
        ErrorsAndWarnings.Should().BeNullOrEmpty();
        SourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }
	
	private static void GenerateKeys(
        IGenerator generator,
        bool publicClass = true,
        bool staticClass = false,
        bool partial = false,
        bool nullForgivingOperators = false,
        bool staticMembers = true
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
using System.Globalization;
using System.Resources;

{(publicClass ? "public" : "internal")}{(staticClass ? " static" : string.Empty)}{(partial ? " partial" : string.Empty)} class ActivityEntrySortRuleNames
{{
    private static ResourceManager? s_resourceManager;
    public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager(""Catglobe.Web.App_GlobalResources.ActivityEntrySortRuleNames"", typeof(ActivityEntrySortRuleNames).Assembly);
    public{(staticMembers ? " static" : string.Empty)} CultureInfo? CultureInfo {{ get; set; }}

    /// <summary>
    /// Looks up a localized string similar to Oldest.
    /// </summary>
    public{(staticMembers ? " static" : string.Empty)} string{(nullForgivingOperators ? string.Empty : "?")} CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo){(nullForgivingOperators ? "!" : string.Empty)};

    /// <summary>
    /// Looks up a localized string similar to Newest.
    /// </summary>
    public{(staticMembers ? " static" : string.Empty)} string{(nullForgivingOperators ? string.Empty : "?")} CreateDateDescending => ResourceManager.GetString(nameof(CreateDateDescending), CultureInfo){(nullForgivingOperators ? "!" : string.Empty)};

    /// <summary>
    /// Looks up a localized string similar to SystemName.
    /// </summary>
    public{(staticMembers ? " static" : string.Empty)} string{(nullForgivingOperators ? string.Empty : "?")} Sys_Name => ResourceManager.GetString(""Sys.Name"", CultureInfo){(nullForgivingOperators ? "!" : string.Empty)};
    public{(staticClass ? " static" : string.Empty)} class Keys
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
}}
";
        var (_, sourceCode, errorsAndWarnings) = generator.Generate(
            options: new FileOptions()
            {
                LocalNamespace = "Catglobe.Web.App_GlobalResources",
                EmbeddedFilename = "Catglobe.Web.App_GlobalResources.ActivityEntrySortRuleNames",
                CustomToolNamespace = "Resources",
                ClassName = "ActivityEntrySortRuleNames",
                PublicClass = publicClass,
                NullForgivingOperators = nullForgivingOperators,
                GroupedFile = new GroupedAdditionalFile(
                    mainFile: new AdditionalTextWithHash(new AdditionalTextStub("", TextWithUnsupportedChar), NewGuid()),
                    subFiles: Array.Empty<AdditionalTextWithHash>()
                ),
                StaticClass = staticClass,
                PartialClass = partial,
                StaticMembers = staticMembers,
				KeyGeneration = true
            }
        );
        errorsAndWarnings.Should().BeNullOrEmpty();
        sourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    [Fact]
    public void Generate_StringBuilder_Public()
    {
        var generator = new StringBuilderGenerator();
        Generate(generator);
        Generate(generator, true, nullForgivingOperators: true);
    }

    [Fact]
    public void Generate_StringBuilder_NonStatic()
    {
        var generator = new StringBuilderGenerator();
        Generate(generator, staticClass: false);
        Generate(generator, staticClass: false, nullForgivingOperators: true);
    }

    [Fact]
    public void Generate_StringBuilder_Internal()
    {
        var generator = new StringBuilderGenerator();
        Generate(generator, false);
        Generate(generator, false, nullForgivingOperators: true);
    }

    [Fact]
    public void Generate_StringBuilder_Partial()
    {
        var generator = new StringBuilderGenerator();
        Generate(generator, partial: true);
        Generate(generator, partial: true, nullForgivingOperators: true);
    }

    [Fact]
    public void Generate_StringBuilder_NonStaticMembers()
    {
        var generator = new StringBuilderGenerator();
        Generate(generator, staticMembers: false);
        Generate(generator, staticMembers: false, nullForgivingOperators: true);
    }

    [Fact]
    public void Generate_StringBuilder_Inner()
    {
        var generator = new StringBuilderGenerator();
        GenerateInner(generator);
    }

    [Fact]
    public void Generate_StringBuilder_InnerInstance()
    {
        var generator = new StringBuilderGenerator();
        GenerateInner(generator, innerClassInstanceName: "Resources", staticMembers: false);
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
namespace Catglobe.Web.App_GlobalResources;
using System.Globalization;
using System.Resources;

public static class CommonMessages
{
    private static ResourceManager? s_resourceManager;
    public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager(""Catglobe.Web.App_GlobalResources.CommonMessages"", typeof(CommonMessages).Assembly);
    public static CultureInfo? CultureInfo { get; set; }

    /// <summary>
    /// Looks up a localized string similar to This entry has been deleted. It is still temporarily accessible, but won&#39;t show up in any of the listings..
    /// </summary>
    public static string? EntryDeleted => ResourceManager.GetString(nameof(EntryDeleted), CultureInfo);

    /// <summary>
    /// Looks up a localized string similar to This entry was merged to.
    /// </summary>
    public static string? EntryMergedTo => ResourceManager.GetString(nameof(EntryMergedTo), CultureInfo);

    /// <summary>
    /// Looks up a localized string similar to Draft = entry is missing crucial information. This status indicates that you&#39;re requesting additional information to be added or corrected.&lt;br /&gt;
    /// Finished = The entry has all the necessary information, but it hasn&#39;t been inspected by a trusted user yet.&lt;br /&gt;
    /// Approved = The entry has been inspected and approved by a trusted user. Approved entries can only be edited by trusted users..
    /// </summary>
    public static string? EntryStatusExplanation => ResourceManager.GetString(nameof(EntryStatusExplanation), CultureInfo);

    /// <summary>
    /// Looks up a localized string similar to This entry is locked, meaning that only moderators are allowed to edit it..
    /// </summary>
    public static string? Locked => ResourceManager.GetString(nameof(Locked), CultureInfo);

    /// <summary>
    /// Looks up a localized string similar to Choose the language for this name. &quot;Original&quot; is the name in original language that isn&#39;t English, for example Japanese. If the original language is English, do not input a name in the &quot;Original&quot; language..
    /// </summary>
    public static string? NameLanguageHelp => ResourceManager.GetString(nameof(NameLanguageHelp), CultureInfo);

    /// <summary>
    /// Looks up a localized string similar to This page revision has been hidden..
    /// </summary>
    public static string? RevisionHidden => ResourceManager.GetString(nameof(RevisionHidden), CultureInfo);
}
";
        var generator = new StringBuilderGenerator();
        var (_, SourceCode, ErrorsAndWarnings) = generator.Generate(
            options: new FileOptions()
            {
                LocalNamespace = "Catglobe.Web.App_GlobalResources",
                EmbeddedFilename = "Catglobe.Web.App_GlobalResources.CommonMessages",
                CustomToolNamespace = null,
                ClassName = "CommonMessages",
                GroupedFile = new GroupedAdditionalFile(
                    mainFile: new AdditionalTextWithHash(new AdditionalTextStub("", text), NewGuid()),
                    subFiles: Array.Empty<AdditionalTextWithHash>()
                ),
                PublicClass = true,
                NullForgivingOperators = false,
                StaticClass = true,
                StaticMembers = true
            }
        );
        ErrorsAndWarnings.Should().BeNullOrEmpty();
        SourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    [Fact]
    public void Generate_StringBuilder_Name_PartialXmlWorks()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <data name=""Works"" xml:space=""preserve"">
    <value>Works.</value>
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
namespace Catglobe.Web.App_GlobalResources;
using System.Globalization;
using System.Resources;

public static class CommonMessages
{
    private static ResourceManager? s_resourceManager;
    public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager(""Catglobe.Web.App_GlobalResources.CommonMessages"", typeof(CommonMessages).Assembly);
    public static CultureInfo? CultureInfo { get; set; }

    /// <summary>
    /// Looks up a localized string similar to Works..
    /// </summary>
    public static string? Works => ResourceManager.GetString(nameof(Works), CultureInfo);
}
";
        var generator = new StringBuilderGenerator();
        var (_, SourceCode, ErrorsAndWarnings) = generator.Generate(
            options: new FileOptions()
            {
                LocalNamespace = "Catglobe.Web.App_GlobalResources",
                EmbeddedFilename = "Catglobe.Web.App_GlobalResources.CommonMessages",
                CustomToolNamespace = null,
                GroupedFile = new GroupedAdditionalFile(
                    mainFile: new AdditionalTextWithHash(new AdditionalTextStub("", text), NewGuid()),
                    subFiles: Array.Empty<AdditionalTextWithHash>()
                ),
                ClassName = "CommonMessages",
                PublicClass = true,
                NullForgivingOperators = false,
                StaticClass = true,
                StaticMembers = true
            }
        );
        ErrorsAndWarnings.Should().BeNullOrEmpty();
        SourceCode.ReplaceLineEndings().Should().Be(expected.ReplaceLineEndings());
    }

    [Fact]
    public void Generate_StringBuilder_Name_DuplicatedataGivesWarning()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <data name=""DupKey"" xml:space=""preserve"">
    <value>Works.</value>
  </data>
  <data name=""DupKey"" xml:space=""preserve"">
    <value>Doeesnt Work.</value>
  </data>
</root>";

        var generator = new StringBuilderGenerator();
        var (_, _, ErrorsAndWarnings) = generator.Generate(
            options: new FileOptions()
            {
                LocalNamespace = "Catglobe.Web.App_GlobalResources",
                EmbeddedFilename = "Catglobe.Web.App_GlobalResources.CommonMessages",
                GroupedFile = new GroupedAdditionalFile(
                    mainFile: new AdditionalTextWithHash(new AdditionalTextStub("", text), NewGuid()),
                    subFiles: Array.Empty<AdditionalTextWithHash>()
                ),
                CustomToolNamespace = null,
                ClassName = "CommonMessages",
                PublicClass = true,
                NullForgivingOperators = false,
                StaticClass = true
            }
        );
        var errs = ErrorsAndWarnings.ToList();
        errs.Should().NotBeNull();
        errs.Should().HaveCount(1);
        errs[0].Id.Should().Be("CatglobeResXFileCodeGenerator001");
        errs[0].Severity.Should().Be(DiagnosticSeverity.Warning);
        errs[0].GetMessage().Should().Contain("DupKey");
        errs[0].Location.GetLineSpan().StartLinePosition.Line.Should().Be(5);
    }

	[Fact]
    public void Generate_StringBuilder_KeyGeneration()
    {
	    var generator = new StringBuilderGenerator();
        GenerateKeys(generator);
        GenerateKeys(generator, true, nullForgivingOperators: true);
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
        var (_, _, ErrorsAndWarnings) = generator.Generate(
            options: new FileOptions()
            {
                LocalNamespace = "Catglobe.Web.App_GlobalResources",
                EmbeddedFilename = "Catglobe.Web.App_GlobalResources.CommonMessages",
                GroupedFile = new GroupedAdditionalFile(
                    mainFile: new AdditionalTextWithHash(new AdditionalTextStub("", text), NewGuid()),
                    subFiles: Array.Empty<AdditionalTextWithHash>()
                ),
                CustomToolNamespace = null,
                ClassName = "CommonMessages",
                PublicClass = true,
                NullForgivingOperators = false,
                StaticClass = true
            }
        );
        var errs = ErrorsAndWarnings.ToList();
        errs.Should().NotBeNull();
        errs.Should().HaveCount(1);
        errs[0].Id.Should().Be("CatglobeResXFileCodeGenerator002");
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
    public void ResxFileName_ShouldNotGenerateIllegalClassnames()
    {
        var ns = Utilities.GetClassNameFromPath("test.cshtml.resx");
        ns.Should().Be("test");
    }
}
