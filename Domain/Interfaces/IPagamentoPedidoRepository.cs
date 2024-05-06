using Domain.Entities;

namespace Domain.Interfaces;

public interface IPagamentoPedidoRepository : IGenericRepository<PagamentoPedido>
{
    Task<PagamentoPedido?> GetByMercadoPagoIdAsync(int mercadoPagoId);
}
