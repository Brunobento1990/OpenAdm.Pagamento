using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public sealed class PagamentoPedidoRepository : GenericRepository<PagamentoPedido>, IPagamentoPedidoRepository
{
    public PagamentoPedidoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }
}
