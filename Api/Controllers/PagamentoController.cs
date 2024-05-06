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
            var result = await _pagamentoSerivce.EfetuarPagamentoAsync(efetuarPagamentoDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPost("notificar")]
    public async Task<IActionResult> Notificar([FromBody]MercadoPagoWebHook? body, [FromQuery]string? cliente)
    {
        Console.WriteLine($"cliente: {cliente ?? "Cliente não encontrado"}");
        var header = HttpContext.Request.Headers["X-Signature"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(header) || string.IsNullOrWhiteSpace(cliente))
        {
            Console.WriteLine("Falhou web hook!");
            return Ok();
        }

        Console.WriteLine($"X-Signature : {header}");

        if (body is not null)
        {
            await _pagamentoSerivce.AtualizarPagamento(body, cliente);
            Console.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
        }

        return Ok();
    }
}
