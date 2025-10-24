using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FilesInfoService
{
    public static string GetFilesInfo(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        var files = directoryInfo.GetFiles();
        var output = new StringBuilder();

        foreach (var file in files)
        {
            output.Append($"Filename: {file.Name}, size: {file.Length / 1024 / 1024 / 1024} GB\n");
        }

        return output.ToString();

    }
}
