using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class NotificationService
{
    public static async Task SendTelegramMessage(string token, string chatId, string message)
    {
        using var client = new HttpClient();
        var url = $"https://api.telegram.org/bot{token}/sendMessage";
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "chat_id", chatId },
            { "text", message }
        });

        await client.PostAsync(url, content);
    }
}
