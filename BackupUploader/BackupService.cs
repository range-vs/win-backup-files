using System;
using System.IO;
using System.IO.Compression;

public class BackupService
{
    private static void CreateZipWithProgress(string sourceDir, string zipPath)
    {
        var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
        int total = files.Length;
        int current = 0;

        using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
        {
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(sourceDir, file);
                zip.CreateEntryFromFile(file, relativePath, CompressionLevel.SmallestSize);

                current++;
                Console.WriteLine($"[{current}/{total}] Archived: {relativePath}");
            }
        }
    }

    public static void CreateZip(string sourceDir, string zipPath)
    {
        if (File.Exists(zipPath))
            File.Delete(zipPath);

        CreateZipWithProgress(sourceDir, zipPath);
    }
}
