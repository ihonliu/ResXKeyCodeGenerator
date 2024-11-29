﻿namespace Ihon.ResXKeyCodeGenerator;

public static class GroupResxFiles
{
    public static IEnumerable<GroupedAdditionalFile> Group(IReadOnlyList<AdditionalTextWithHash> allFilesWithHash,
        CancellationToken cancellationToken = default)
    {
        var lookup = new Dictionary<string, AdditionalTextWithHash>();
        var res = new Dictionary<AdditionalTextWithHash, List<AdditionalTextWithHash>>();
        foreach (var file in allFilesWithHash)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var path = file.File.Path;
            var pathName = Path.GetDirectoryName(path);
            var baseName = Utilities.GetBaseName(path);
            if (Path.GetFileNameWithoutExtension(path) == baseName)
            {
                var key = pathName + "\\" + baseName;
                //it should be impossible to exist already, but VS sometimes throws error about duplicate key added. Keep the original entry, not the new one
                if (!lookup.ContainsKey(key))
                    lookup.Add(key, file);
                res.Add(file, []);
            }
        }

        foreach (var fileWithHash in allFilesWithHash)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var path = fileWithHash.File.Path;
            var pathName = Path.GetDirectoryName(path);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var baseName = Utilities.GetBaseName(path);
            if (fileNameWithoutExtension == baseName)
                continue;
            // this might happen if a .nn.resx file exists without a .resx file
            if (!lookup.TryGetValue(pathName + "\\" + baseName, out var additionalText))
                continue;
            res[additionalText].Add(fileWithHash);
        }

        // don't care at all HOW it is sorted, just that end result is the same
        foreach (var file in res)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return new GroupedAdditionalFile(file.Key, file.Value);
        }
    }
}
