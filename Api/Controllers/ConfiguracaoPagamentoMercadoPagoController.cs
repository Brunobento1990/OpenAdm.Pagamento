using Application.Dtos.ConfiguracoesPagamentosMercadoPago;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("configuracao-pagamento-mercado-pago")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ConfiguracaoPagamentoMercadoPagoController : ControllerBaseApi
{
    private readonly IConfiguracoesPagamentosMercadoPagoService _service;

    public ConfiguracaoPagamentoMercadoPagoController(IConfiguracoesPagamentosMercadoPagoService service)
    {
        _service = service;
    }

    [HttpPost("create-or-update")]
    public async Task<IActionResult> CreateOrUpdate(CreateConfiguracoesPagamentosMercadoPagoDto createConfiguracoesPagamentosMercadoPagoDto)
    {
        try
        {
            var response = await _service.CreateOrUpdateAsync(createConfiguracoesPagamentosMercadoPagoDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var response = await _service.GetAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("cobrar")]
    public async Task<IActionResult> Cobrar()
    {
        try
        {
            var cobrar = await _service.CobrarAsync();
            return Ok(new { cobrar });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
