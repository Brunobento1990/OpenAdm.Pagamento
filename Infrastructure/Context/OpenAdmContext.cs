using Domain.Entities;
using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Context;

public class OpenAdmContext(DbContextOptions<OpenAdmContext> options) 
    : DbContext(options)
{
    public DbSet<Parceiro> Parceiros { get; set; }
    public DbSet<ConfiguracaoParceiro> ConfiguracoesParceiro { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
