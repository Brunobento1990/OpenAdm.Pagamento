using Domain.Entities;
using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Context;

public class ContextMercadoPago : DbContext
{
    public ContextMercadoPago(DbContextOptions<ContextMercadoPago> options) : base(options)
    {
    }

    public DbSet<ConfiguracaoPagamentoMercadoPago> ConfiguracoesPagamentosMercadoPago { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<ItensPedido> ItensPedidos { get; set; }
    public DbSet<PagamentoPedido> PagamentosPedidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
