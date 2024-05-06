using Infrastructure.Model;

namespace Infrastructure.HttpService.Interfaces;

public interface IDiscordHttpService
{
    Task NotifyExceptionAsync(DiscordModel discordModel, string webHookId, string webHookToken);
}
