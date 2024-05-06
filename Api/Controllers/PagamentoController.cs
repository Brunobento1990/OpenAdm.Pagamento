using Application.Dtos.MercadoPago;
using Application.Dtos.Pagamentos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("notificar")]
    public async Task<IActionResult> Notificar([FromBody]object? body, [FromQuery]string? cliente)
    {
        Console.WriteLine($"cliente: {cliente ?? "Cliente não encontrado"}");
        var header = HttpContext.Request.Headers["X-Signature"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(header) || string.IsNullOrWhiteSpace(cliente))
        {
            Console.WriteLine("Falhou web hook!");
            return Ok();
        }

        Console.WriteLine($"Body: {JsonSerializer.Serialize(body)}");

        Console.WriteLine($"X-Signature : {header}");

        if (body is MercadoPagoWebHook mercadoPagoRequest 
            && mercadoPagoRequest.Data is not null
            && mercadoPagoRequest.Action == "payment.updated")
        {
            await _pagamentoSerivce.AtualizarPagamento(mercadoPagoRequest, cliente);
            Console.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
        }

        return Ok();
    }
}
