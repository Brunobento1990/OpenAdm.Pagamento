using Api.Configuracoes;
using Domain.Pkg.Cryptography;
using dotenv.net;
using Infrastructure.Model;
using IoC.Application;
using IoC.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Services.ConfigureController();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var key = VariaveisDeAmbiente.GetVariavel("CRYP_KEY");
var iv = VariaveisDeAmbiente.GetVariavel("CRYP_IV");
var pgString = VariaveisDeAmbiente.GetVariavel("STRING_CONNECTION");
var urlDiscord = VariaveisDeAmbiente.GetVariavel("URL_DISCORD");
var urlMercadoPago = VariaveisDeAmbiente.GetVariavel("URL_MERCADO_PAGO");
var keyJwt = VariaveisDeAmbiente.GetVariavel("JWT_KEY");
var issue = VariaveisDeAmbiente.GetVariavel("JWT_ISSUE");
var audience = VariaveisDeAmbiente.GetVariavel("JWT_AUDIENCE");

var httpModel = new List<HttpClientInjectModel>()
{
    new()
    {
        UrlBase = urlDiscord,
        HttpClient = "Discord"
    },
    new()
    {
        UrlBase = urlMercadoPago,
        HttpClient = "MercadoPago"
    }
};

CryptographyGeneric.Configure(key, iv);

builder.Services
    .InjectContext(pgString)
    .InjectRepositories()
    .InjectServices()
    .InjectCors()
    .HttpClientInject(httpModel)
    .PagamentoInject()
    .InjectJwt(keyJwt, issue, audience);

var app = builder.Build();

var basePath = "/api/v1/pagamento";
app.UsePathBase(new PathString(basePath));

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
        });
    });
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("base");

app.MapControllers();

app.Run();
