using Domain.Interfaces;
using Domain.Pkg.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class PedidoRepository : IPedidoRepository
{
    private readonly ParceiroContext _parceiroContext;

    public PedidoRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<Pedido?> GetByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .Include(x => x.ItensPedido)
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
