namespace Ihon.ResXKeyCodeGenerator;

public sealed partial class StringBuilderGenerator : IGenerator
{
    private static void CreateMemberKey(string indent,
        StringBuilder builder,
        FileOptions options,
        string name,
        IXmlLineInfo line,
        HashSet<string> alreadyAddedMembers,
        List<Diagnostic> errorsAndWarnings,
        string containerClassName)
    {
        if (!GenerateMemberKey(indent, builder, options, name, line, alreadyAddedMembers, errorsAndWarnings,
                containerClassName, out var resourceAccessByName))
        {
            return;
        }

        if (resourceAccessByName)
        {
            // => name
            builder.Append(" = nameof(");
            builder.Append(name);
            builder.Append(")");
        }
        else
        {
            // => "name"
            // replace " with \"
            builder.Append(" = \"");
            builder.Append(name.Replace(@"""", @"\"""));
            builder.Append("\"");
        }

        builder.AppendLineLf(";");
    }

    private static bool GenerateMemberKey(string indent,
        StringBuilder builder,
        FileOptions options,
        string name,
        IXmlLineInfo line,
        HashSet<string> alreadyAddedMembers,
        List<Diagnostic> errorsAndWarnings,
        string containerClassName,
        out bool resourceAccessByName)
    {
        string memberName;

        if (ValidMemberNamePattern.IsMatch(name))
        {
            memberName = name;
            resourceAccessByName = true;
        }
        else
        {
            memberName = InvalidMemberNameSymbols.Replace(name, "_");
            resourceAccessByName = false;
        }

        static Location GetMemberLocation(FileOptions fileOptions, IXmlLineInfo line, string memberName)
        {
            return Location.Create(
                fileOptions.GroupedFile.MainFile.File.Path,
                new TextSpan(),
                new LinePositionSpan(
                    new LinePosition(line.LineNumber - 1, line.LinePosition - 1),
                    new LinePosition(line.LineNumber - 1, line.LinePosition - 1 + memberName.Length)
                )
            );
        }

        if (!alreadyAddedMembers.Add(memberName))
        {
            errorsAndWarnings.Add(Diagnostic.Create(
                DuplicateWarning,
                GetMemberLocation(options, line, memberName), memberName
            ));
            return false;
        }

        if (memberName == containerClassName)
        {
            errorsAndWarnings.Add(Diagnostic.Create(
                MemberSameAsClassWarning,
                GetMemberLocation(options, line, memberName), memberName
            ));
            return false;
        }

        builder.AppendLineLf();

        builder.Append(indent);
        builder.AppendLineLf("/// <summary>");

        builder.Append(indent);
        builder.Append("/// Name of resource ");
        builder.Append(name);
        builder.AppendLineLf(".");

        builder.Append(indent);
        builder.AppendLineLf("/// </summary>");

        builder.Append(indent);
        builder.Append("public ");
        builder.Append("const string ");
        builder.Append(memberName);
        return true;
    }

    private void KeyGeneration(
        FileOptions options,
        SourceText content,
        string indent,
        string containerClassName,
        StringBuilder builder,
        List<Diagnostic> errorsAndWarnings,
        CancellationToken cancellationToken
    )
    {
        var members = ReadResxFile(content);
        if (members is null)
        {
            return;
        }

        var alreadyAddedMembers = new HashSet<string> { Constants.CultureInfoVariable };
        foreach (var (key, value, line) in members)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CreateMemberKey(
                indent,
                builder,
                options,
                key,
                line,
                alreadyAddedMembers,
                errorsAndWarnings,
                containerClassName
            );
        }
    }
}
