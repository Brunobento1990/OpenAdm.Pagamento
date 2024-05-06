using Domain.Entities;
using Domain.Interfaces;
using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Context;

public class ParceiroContext(DbContextOptions<ParceiroContext> options,
    IConfiguracaoParceiroRepository configuracaoParceiroRepository)
    : DbContext(options)
{
    private readonly IConfiguracaoParceiroRepository _configuracaoParceiroRepository = configuracaoParceiroRepository;
    public DbSet<ConfiguracaoPagamentoMercadoPago> ConfiguracoesPagamentosMercadoPago { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<ItensPedido> ItensPedidos { get; set; }
    public DbSet<PagamentoPedido> PagamentosPedidos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuracaoParceiroRepository.GetConexaoDbByDominioAsync().Result;
        optionsBuilder.UseNpgsql(connectionString);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
