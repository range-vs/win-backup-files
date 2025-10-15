using System;
using System.Diagnostics;
using System.IO;

public class UploadService
{
    public static void MountSMB(SmbConfig config)
    {
        // Проверка: если путь уже доступен — не монтируем
        if (Directory.Exists(config.Host))
        {
            Console.WriteLine("SMB is mounted");
            return;
        }
        var psi = new ProcessStartInfo { FileName = "net", Arguments = $"use \"{config.Host}\" /user:{config.Username} {config.Password}", RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true };
        using var process = Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new Exception($"Error mount SMB: {error}");
        }
        Console.WriteLine("SMB mount complete");
    }
    public static void UploadToSMB(string filePath, SmbConfig config)
    {
        var networkPath = $"{config.Host}\\{Path.GetFileName(filePath)}";
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var dest = new FileStream(networkPath, FileMode.Create, FileAccess.Write);
        fs.CopyTo(dest);
    }
}