using Api.Attributes;
using Application.Dtos.Pagamentos;
using Application.Interfaces;
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
    public async Task<IActionResult> Notificar([FromBody] object body, [FromQuery] string cliente)
    {
        Console.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
        Type type = body.GetType();
        var properties = type.GetProperties();

        var data = properties.FirstOrDefault(x => x.Name.ToLower() == "data")?.GetValue(body);
        var action = properties.FirstOrDefault(x => x.Name.ToLower() == "action")?.GetValue(body)?.ToString();
        Console.WriteLine($"data: {JsonSerializer.Serialize(data)}");
        Console.WriteLine($"action: {action}");
        if (data is not null && (action == "payment.update" || action == "payment.updated"))
        {
            Console.WriteLine($"Data: {JsonSerializer.Serialize(data)}");
            var typeData = body.GetType();
            var propertiesData = type.GetProperties();
            var mercadoPagoId = propertiesData.FirstOrDefault(x => x.Name == "id")?.GetValue(data);
            if (mercadoPagoId != null)
            {
                await _pagamentoSerivce.AtualizarPagamento((long)mercadoPagoId);
                Console.WriteLine("Processamento concluido com sucesso!");
            }
            else
            {
                Console.WriteLine("Não conseguiu dar o parse no data.id");
            }
        }
        else
        {
            Console.WriteLine($"Não achou o body: {JsonSerializer.Serialize(body)}");
        }

        return Ok();
    }
}
