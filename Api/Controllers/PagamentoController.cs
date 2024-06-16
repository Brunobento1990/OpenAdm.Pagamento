using Api.Attributes;
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

    [NotificacaoMercadoPago]
    [HttpPost("notificar")]
    public async Task<IActionResult> Notificar([FromBody] object? body, [FromQuery] string cliente)
    {
        Console.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
        //if (body?.Data is not null && body?.Action == "payment.update")
        //{
        //    await _pagamentoSerivce.AtualizarPagamento(body, cliente);
        //    Console.WriteLine($"Body: {JsonSerializer.Serialize(body)}");
        //}
        //else
        //{
        //    Console.WriteLine($"Não achou o body: {JsonSerializer.Serialize(body)}");
        //}

        return Ok();
    }
}
