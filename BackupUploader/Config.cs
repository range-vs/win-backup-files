public class Config
{
    public string SourceFolder { get; set; }
    public string ZipOutput { get; set; }
    public SmbConfig SMB { get; set; }
    public WebDavConfig WebDAV { get; set; }
    public TelegramConfig Telegram { get; set; }
    public string InterfaceIP { get; set; }
}

public class SmbConfig
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class WebDavConfig
{
    public string Url { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class TelegramConfig
{
    public string BotToken { get; set; }
    public string ChatId { get; set; }
}
