using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

string workingDir = Environment.CurrentDirectory;

var configuration = new ConfigurationBuilder()
            .AddJsonFile($"{workingDir}\\appsettings.json")
            .Build();

var config = new Config
{
    SourceFolder = configuration["SourceFolder"],
    ZipOutput = configuration["ZipOutput"],
    SMB = new SmbConfig
    {
        Host = configuration["SMB:Host"],
        Username = configuration["SMB:Username"],
        Password = configuration["SMB:Password"]
    },
    WebDAV = new WebDavConfig
    {
        Url = configuration["WebDAV:Url"],
        Username = configuration["WebDAV:Username"],
        Password = configuration["WebDAV:Password"]
    },
    Telegram = new TelegramConfig
    {
        BotToken = configuration["Telegram:BotToken"],
        ChatId = configuration["Telegram:ChatId"]
    },
    InterfaceIP = configuration["InterfaceIP"]
};

string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
string fileName = $"father_music_bac_{timestamp}.zip";
var zipOutputFolder = config.ZipOutput;
config.ZipOutput += fileName;
var message = "";

try
{
    Console.WriteLine("Check SMB...");
    UploadService.MountSMB(config.SMB);

    Console.WriteLine("Archiving...");
    BackupService.CreateZip(config.SourceFolder, config.ZipOutput);

    Console.WriteLine("Upload to SMB...");
    UploadService.UploadToSMB(config.ZipOutput, config.SMB);

    Console.WriteLine("Clear old backups from disk...");
    ClearOldBackupsService.CleanupOldBackups(zipOutputFolder);

    Console.WriteLine("Clear old backups from SMB...");
    ClearOldBackupsService.CleanupOldBackups(config.SMB);

    //Console.WriteLine("Upload to WebDAV...");
    //await UploadService.UploadToWebDav(config.ZipOutput, config.WebDAV);


    message = $"father music backup ok\nDisk\n{FilesInfoService.GetFilesInfo(zipOutputFolder)}\n---\nSMB\n{FilesInfoService.GetFilesInfo(config.SMB.Host)}";

}
catch (Exception ex)
{
    message = $"Error {ex.Message}";
}
finally
{
    await NotificationService.SendTelegramMessage(config.Telegram.BotToken, config.Telegram.ChatId, message);
    Console.WriteLine(message);
}
