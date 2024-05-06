using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public sealed class ConfiguracaoPagamentoMercadoPagoConfiguration
    : IEntityTypeConfiguration<ConfiguracaoPagamentoMercadoPago>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoPagamentoMercadoPago> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataDeCriacao)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.DataDeAtualizacao)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.Numero)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.PublicKey)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(x => x.AccessToken)
            .IsRequired()
            .HasMaxLength(1000);
    }
}
