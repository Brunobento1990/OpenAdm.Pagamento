using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class PagamentoPedidoRepository : GenericRepository<PagamentoPedido>, IPagamentoPedidoRepository
{
    private readonly ParceiroContext _context;
    public PagamentoPedidoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _context = parceiroContext;
    }

    public async Task<PagamentoPedido?> GetByMercadoPagoIdAsync(int mercadoPagoId)
    {
        return await _context
            .PagamentosPedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MercadoPagoId == mercadoPagoId);
    }
}
