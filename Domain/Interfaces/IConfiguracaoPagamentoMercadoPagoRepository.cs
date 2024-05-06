using Domain.Entities;

namespace Domain.Interfaces;

public interface IConfiguracaoPagamentoMercadoPagoRepository : IGenericRepository<ConfiguracaoPagamentoMercadoPago>
{
    Task<ConfiguracaoPagamentoMercadoPago?> GetAsync();
}
