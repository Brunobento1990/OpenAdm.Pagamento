using Domain.Pkg.Entities;

namespace Domain.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> GetByIdAsync(Guid id);
}
