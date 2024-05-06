using Domain.Entities;
using Domain.Pkg.Cryptography;

namespace Application.Models.ConfiguracoesPagamentosMercadoPago;

public class ConfiguracaoPagamentoMercadoPagoViewModel : BaseViewModel
{
    public string PublicKey { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public bool? CobrarCpf { get; set; }
    public bool? CobrarCnpj { get; set; }

    public ConfiguracaoPagamentoMercadoPagoViewModel ToModel(ConfiguracaoPagamentoMercadoPago config)
    {
        Id = config.Id;
        PublicKey = CryptographyGeneric.Decrypt(config.PublicKey);
        DataDeCriacao = config.DataDeCriacao;
        DataDeAtualizacao = config.DataDeAtualizacao;
        Numero = config.Numero;
        AccessToken = CryptographyGeneric.Decrypt(config.AccessToken);
        CobrarCnpj = config.CobrarCnpj;
        CobrarCpf = config.CobrarCpf;

        return this;
    }
}
