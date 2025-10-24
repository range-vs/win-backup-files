using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

public class ClearOldBackupsService
{
    public static void CleanupOldBackups(SmbConfig config, int keepLast = 3)
    {
        var directory = new DirectoryInfo(config.Host);
        var files = directory.GetFiles("father_music_bac_*.zip")
            .Select(f => new
            {
                File = f,
                FileName = f.Name,
                Match = Regex.Match(f.Name, @"father_music_bac_(\d{8}_\d{6})\.zip")
            })
            .Where(x => x.Match.Success)
            .Select(x => new
            {
                x.File,
                x.FileName,
                Timestamp = DateTime.ParseExact(x.Match.Groups[1].Value, "yyyyMMdd_HHmmss", null)
            })
            .OrderByDescending(x => x.Timestamp)
            .ToList();

        foreach (var file in files.Skip(keepLast))
        {
            file.File.Delete();
            Console.WriteLine($"Deleted: {file.FileName}");
        }
        
    }

    public static void CleanupOldBackups(string directoryPath, int keepLast = 3)
    {
        var directory = new DirectoryInfo(directoryPath);
        if (!directory.Exists)
            return;

        var files = directory.GetFiles("father_music_bac_*.zip")
            .Select(f => new
            {
                File = f,
                FileName = f.Name,
                Match = Regex.Match(f.Name, @"father_music_bac_(\d{8}_\d{6})\.zip")
            })
            .Where(x => x.Match.Success)
            .Select(x => new
            {
                x.File,
                x.FileName,
                Timestamp = DateTime.ParseExact(x.Match.Groups[1].Value, "yyyyMMdd_HHmmss", null)
            })
            .OrderByDescending(x => x.Timestamp)
            .ToList();

        foreach (var file in files.Skip(keepLast))
        {
            file.File.Delete();
            Console.WriteLine($"Deleted: {file.FileName}");
        }
    }
    
}
