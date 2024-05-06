using Domain.Interfaces;
using Domain.Model;
using Domain.Pkg.Cryptography;
using Infrastructure.HttpService.Interfaces;

namespace Infrastructure.Pagamento.Services;

public sealed class PagamentoViaPix : IPagamento
{
    private readonly IMercadoPagoHttpClient _mercadoPagoHttpClient;
    private readonly IConfiguracaoPagamentoMercadoPagoRepository _configuracaoPagamentoMercadoPagoRepository;

    public PagamentoViaPix(
        IMercadoPagoHttpClient mercadoPagoHttpClient,
        IConfiguracaoPagamentoMercadoPagoRepository configuracaoPagamentoMercadoPagoRepository)
    {
        _mercadoPagoHttpClient = mercadoPagoHttpClient;
        _configuracaoPagamentoMercadoPagoRepository = configuracaoPagamentoMercadoPagoRepository;
    }

    public async Task<ResultPagamento> PagarAsync(MercadoPagoRequest mercadoPagoRequest)
    {
        var config = await _configuracaoPagamentoMercadoPagoRepository.GetAsync()
            ?? throw new Exception("Configuração de pagamento não configurada!");

        if (string.IsNullOrWhiteSpace(config.AccessToken))
        {
            throw new Exception("Configuração de pagamento inválida!");
        }
        
        var result = await _mercadoPagoHttpClient.PostAsync(
            mercadoPagoRequest, 
            CryptographyGeneric.Decrypt(config.AccessToken));

        return new ResultPagamento()
        {
            LinkPagamento = result.Point_of_interaction?.Transaction_data?.Ticket_url,
            QrCodePixBase64 = result.Point_of_interaction?.Transaction_data?.Qr_code_base64,
            QrCodePix = result.Point_of_interaction?.Transaction_data?.Qr_code,
            MercadoPagoId = result.Id
        };
    }
}
