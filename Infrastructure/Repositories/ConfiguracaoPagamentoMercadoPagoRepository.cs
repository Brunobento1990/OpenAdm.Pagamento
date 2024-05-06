using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class ConfiguracaoPagamentoMercadoPagoRepository
    : GenericRepository<ConfiguracaoPagamentoMercadoPago>, IConfiguracaoPagamentoMercadoPagoRepository
{
    private readonly ParceiroContext _context;
    public ConfiguracaoPagamentoMercadoPagoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _context = parceiroContext;
    }

    public async Task<ConfiguracaoPagamentoMercadoPago?> GetAsync()
    {
        return await _context
            .ConfiguracoesPagamentosMercadoPago
            .AsNoTracking()
            .OrderBy(x => x.Numero)
            .FirstOrDefaultAsync();
    }
}
