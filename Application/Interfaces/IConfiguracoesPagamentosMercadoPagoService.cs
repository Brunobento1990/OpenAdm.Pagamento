using Application.Dtos.ConfiguracoesPagamentosMercadoPago;
using Application.Models.ConfiguracoesPagamentosMercadoPago;

namespace Application.Interfaces;

public interface IConfiguracoesPagamentosMercadoPagoService
{
    Task<ConfiguracaoPagamentoMercadoPagoViewModel> CreateOrUpdateAsync(
        CreateConfiguracoesPagamentosMercadoPagoDto createConfiguracoesPagamentosMercadoPagoDto);
    Task<ConfiguracaoPagamentoMercadoPagoViewModel> GetAsync();
    Task<bool> CobrarAsync();
}
