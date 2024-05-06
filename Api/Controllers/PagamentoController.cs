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
    public IActionResult Notificar(object? body)
    {
        var header = HttpContext.Request.Headers;

        Console.WriteLine($"Header : {JsonSerializer.Serialize(header)}");

        if(body is not null)
        {
            Console.WriteLine(JsonSerializer.Serialize(body));
        }

        return Ok();
    }
}
