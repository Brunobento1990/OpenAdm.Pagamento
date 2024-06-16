using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Infrastructure.Context;
using Microsoft.AspNetCore.WebUtilities;
using Domain.Pkg.Cryptography;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Api.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public class NotificacaoMercadoPagoAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, 
        ActionExecutionDelegate next)
    {
        var serviceProvider = context.HttpContext.RequestServices;
        var openAdmContext = serviceProvider.GetRequiredService<OpenAdmContext>();
        var notificacaoMercadoPagoModel = serviceProvider.GetRequiredService<INotificacaoMercadoPagoModel>();

        var request = context.HttpContext.Request;
        var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host)
        {
            Path = request.Path,
            Query = request.QueryString.ToString()
        };

        var query = QueryHelpers.ParseQuery(uriBuilder.Query);

        if (!query.ContainsKey("cliente"))
        {
            Console.WriteLine("Não encontrou a string da query params");
            Result(context);
            return;
        }

        var cliente = query.FirstOrDefault(x => x.Key == "cliente").Value;
        var header = context.HttpContext.Request.Headers["X-Signature"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(header) || string.IsNullOrWhiteSpace(cliente))
        {
            Console.WriteLine("Falhou webhook mercado pago!");
            Result(context);
            return;
        }

        var configuracaoParceiro = await openAdmContext
            .ConfiguracoesParceiro
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClienteMercadoPago == cliente.ToString());

        if (configuracaoParceiro == null)
        {
            Console.WriteLine("Não encontrou a configuração do parceiro!");
            Result(context);
            return;
        }

        notificacaoMercadoPagoModel.ConnectionString = CryptographyGeneric.Decrypt(configuracaoParceiro.ConexaoDb);
        Console.WriteLine("Processou o midleware com sucesso");
        await next();
    }

    private static void Result(ActionExecutingContext context)
    {
        context.Result = new ContentResult()
        {
            StatusCode = 200,
            Content = JsonSerializer.Serialize(new { message = "Requisição recebida com sucesso!" })
        };
    }
}
