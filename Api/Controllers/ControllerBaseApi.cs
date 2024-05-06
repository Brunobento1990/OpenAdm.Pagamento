using Api.Configuracoes;
using Domain.Pkg.Exceptions;
using Infrastructure.HttpService.Interfaces;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public abstract class ControllerBaseApi : ControllerBase
{
    protected async Task<IActionResult> HandleErrorAsync(Exception ex)
    {
        if (ex.GetType() == typeof(ExceptionApi) || VariaveisDeAmbiente.GetVariavel("AMBIENTE").Equals("develop"))
            return BadRequest(new { ex.Message });

        var discordHttpService = HttpContext.RequestServices.GetRequiredService<IDiscordHttpService>();

        var webHookId = VariaveisDeAmbiente.GetVariavel("DISCORD_WEB_HOOK_ID");
        var webHookToken = VariaveisDeAmbiente.GetVariavel("DISCORD_WEB_HOOK_TOKEN");

        var discordModel = new DiscordModel()
        {
            Content = "Error expeptions",
            Username = "Error",
            Embeds =
            [
                new()
                {
                    Description = ex.Message,
                    Title = "Error api",
                    Color = 0xFF0000
                }
            ]
        };

        await discordHttpService.NotifyExceptionAsync(discordModel, webHookId, webHookToken);

        return BadRequest(new { Message = "Ocorreu um erro interno, tente novamente mais tarde, ou entre em contato com o suporte!" });
    }
}
