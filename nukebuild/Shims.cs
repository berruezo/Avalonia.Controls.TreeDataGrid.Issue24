#nullable disable
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;

public partial class Build
{
    static void Information(string info)
    {
        Logger.Info(info);
    }

    static void Information(string info, params object[] args)
    {
        Logger.Info(info, args);
    }

    private void Zip(AbsolutePath target, params string[] paths) => Zip(target, paths.AsEnumerable());

    private void Zip(AbsolutePath target, IEnumerable<string> paths)
    {
        var targetPath = target.ToString();
        bool finished = false, atLeastOneFileAdded = false;
        try
        {
            using (var targetStream = File.Create(targetPath))
            using(var archive = new System.IO.Compression.ZipArchive(targetStream, ZipArchiveMode.Create))
            {
                void AddFile(string path, string relativePath)
                {
                    var e = archive.CreateEntry(relativePath.Replace("\\", "/"), CompressionLevel.Optimal);
                    using (var entryStream = e.Open())
                    using (var fileStream = File.OpenRead(path))
                        fileStream.CopyTo(entryStream);
                    atLeastOneFileAdded = true;
                }

                foreach (var path in paths)
                {
                    if (Directory.Exists(path))
                    {
                        var dirInfo = new DirectoryInfo(path);
                        var rootPath = Path.GetDirectoryName(dirInfo.FullName);
                        foreach(var fsEntry in dirInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
                        {
                            if (fsEntry is FileInfo)
                            {
                                var relPath = Path.GetRelativePath(rootPath, fsEntry.FullName);
                                AddFile(fsEntry.FullName, relPath);
                            }
                        }
                    }
                    else if(File.Exists(path))
                    {
                        var name = Path.GetFileName(path);
                        AddFile(path, name);
                    }
                }
            }

            finished = true;
        }
        finally
        {
            try
            {
                if (!finished || !atLeastOneFileAdded)
                    File.Delete(targetPath);
            }
            catch
            {
                //Ignore
            }
        }
    }
}
