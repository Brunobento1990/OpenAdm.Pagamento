using Domain.Entities;
using Domain.Pkg.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ConfiguracoesPagamentosMercadoPago;

public class CreateConfiguracoesPagamentosMercadoPagoDto
{
    [Required]
    public string PublicKey { get; set; } = string.Empty;
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    public bool? CobrarCpf { get; set; }
    public bool? CobrarCnpj { get; set; }

    public ConfiguracaoPagamentoMercadoPago ToEntity()
    {
        var date = DateTime.Now;
        return new ConfiguracaoPagamentoMercadoPago(
            id: Guid.NewGuid(),
            dataDeCriacao: date,
            dataDeAtualizacao: date,
            numero: 0,
            publicKey: CryptographyGeneric.Encrypt(PublicKey),
            accessToken: CryptographyGeneric.Encrypt(AccessToken),
            cobrarCpf: CobrarCpf,
            cobrarCnpj: CobrarCnpj);
    }
}
