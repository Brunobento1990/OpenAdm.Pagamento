using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Api.Configuracoes;

public static class ConfigureControllerService
{
    public static void ConfigureController(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}
