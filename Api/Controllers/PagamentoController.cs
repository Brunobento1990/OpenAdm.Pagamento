﻿using Api.Attributes;
using Application.Dtos.Pagamentos;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace Api.Controllers;

[ApiController]
[Route("pagamento")]
public class PagamentoController : ControllerBaseApi
{
    private readonly IPagamentoSerivce _pagamentoSerivce;

    public PagamentoController(IPagamentoSerivce pagamentoSerivce)
    {
        _pagamentoSerivce = pagamentoSerivce;
    }

    [HttpPost]
    public async Task<IActionResult> Pagar(EfetuarPagamentoDto efetuarPagamentoDto)
    {
        try
        {
            var referer = HttpContext.Request.Headers["Referer"].FirstOrDefault();
            var result = await _pagamentoSerivce.EfetuarPagamentoAsync(efetuarPagamentoDto, referer ?? "");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [NotificacaoMercadoPago]
    [HttpPost("notificar")]
    public async Task<IActionResult> Notificar([FromBody] Notification body)
    {
        Console.WriteLine($"Resource: {body.Resource}");
        Console.WriteLine($"Topic: {body.Topic}");

        if (body.Resource != null)
        {
            var id = body.Resource.Replace("https://api.mercadolibre.com/collections/notifications/", "");
            if (!string.IsNullOrWhiteSpace(id))
            {
                await _pagamentoSerivce.AtualizarPagamento(long.Parse(id));
                Console.WriteLine("Processamento concluído com sucesso!");
            }
            else
            {
                Console.WriteLine("Não conseguiu dar o parse no resource ID");
            }
        }
        else
        {
            Console.WriteLine($"Não achou o body: {JsonSerializer.Serialize(body)}");
        }
        //Console.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
        //Type type = body.GetType();
        //var properties = type.GetProperties();

        //var data = properties.FirstOrDefault(x => x.Name.ToLower() == "data")?.GetValue(body);
        //var action = properties.FirstOrDefault(x => x.Name.ToLower() == "action")?.GetValue(body)?.ToString();
        //Console.WriteLine($"data: {JsonSerializer.Serialize(data)}");
        //Console.WriteLine($"action: {action}");
        //if (data is not null && (action == "payment.update" || action == "payment.updated"))
        //{
        //    Console.WriteLine($"Data: {JsonSerializer.Serialize(data)}");
        //    var typeData = body.GetType();
        //    var propertiesData = type.GetProperties();
        //    var mercadoPagoId = propertiesData.FirstOrDefault(x => x.Name == "id")?.GetValue(data);
        //    if (mercadoPagoId != null)
        //    {
        //        await _pagamentoSerivce.AtualizarPagamento((long)mercadoPagoId);
        //        Console.WriteLine("Processamento concluido com sucesso!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Não conseguiu dar o parse no data.id");
        //    }
        //}
        //else
        //{
        //    var resorce = properties.FirstOrDefault(x => x.Name.ToLower() == "resource")?.GetValue(body);
        //    Console.WriteLine($"resorce: ", resorce);
        //    if (resorce != null)
        //    {
        //        var id = resorce.ToString()?.Replace("https://api.mercadolibre.com/collections/notifications/", "");
        //        if (!string.IsNullOrWhiteSpace(id))
        //        {
        //            await _pagamentoSerivce.AtualizarPagamento(long.Parse(id));
        //            Console.WriteLine("Processamento concluido com sucesso!");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Não achou o body: {JsonSerializer.Serialize(body)}");
        //    }
        //}

        return Ok();
    }
}

public class Notification
{
    public string Resource { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
}
