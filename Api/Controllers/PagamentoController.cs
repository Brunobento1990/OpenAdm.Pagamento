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
    public async Task<IActionResult> Notificar([FromBody] Notification body)
    {
        if (body?.Data != null && (body?.Action == "payment.update" || body?.Action == "payment.updated"))
        {
            if (!string.IsNullOrWhiteSpace(body?.Data?.Id))
            {
                var tryParseId = long.TryParse(body.Data.Id, out long newId);
                if (tryParseId)
                {
                    await _pagamentoSerivce.AtualizarPagamento(newId);
                    Console.WriteLine("Processamento concluído com sucesso!");
                }
                else
                {
                    Console.WriteLine("Não conseguiu dar o parse no resource ID");
                }
            }
            else
            {
                Console.WriteLine("Não ha ID no Data");
            }
        }
        else
        {
            Console.WriteLine($"Não achou o body: {JsonSerializer.Serialize(body)}");
        }

        return Ok();
    }
}
