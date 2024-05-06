using Domain.Entities;

namespace Test.Builder;

public class ConfiguracaoPagamentoMercadoPagoBuilder
{
    private string _publicKey = string.Empty;
    private string _accessToken = string.Empty;
    private bool? _cobrarCnpj;
    private bool? _cobrarCpf;
    public static ConfiguracaoPagamentoMercadoPagoBuilder Init() => new();

    public ConfiguracaoPagamentoMercadoPagoBuilder AddPublicKey(string publicKey)
    {
        _publicKey = publicKey;
        return this;
    }

    public ConfiguracaoPagamentoMercadoPagoBuilder AddAccessToken(string accessToken)
    {
        _accessToken = accessToken;
        return this;
    }

    public ConfiguracaoPagamentoMercadoPago Builder()
    {
        return new ConfiguracaoPagamentoMercadoPago(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now,
            1,
            _publicKey,
            _accessToken,
            _cobrarCpf,
            _cobrarCnpj);
    }
}
