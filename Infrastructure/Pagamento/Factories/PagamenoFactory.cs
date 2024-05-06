using Domain.Enuns;
using Domain.Factories;
using Domain.Interfaces;
using Infrastructure.HttpService.Interfaces;
using Infrastructure.Pagamento.Services;

namespace Infrastructure.Pagamento.Factories;

public sealed class PagamenoFactory : IPagamenoFactory
{
    private readonly IMercadoPagoHttpClient _mercadoPagoHttpClient;
    private readonly IConfiguracaoPagamentoMercadoPagoRepository _configuracaoPagamentoMercadoPagoRepository;

    public PagamenoFactory(
        IMercadoPagoHttpClient mercadoPagoHttpClient,
        IConfiguracaoPagamentoMercadoPagoRepository configuracaoPagamentoMercadoPagoRepository)
    {
        _mercadoPagoHttpClient = mercadoPagoHttpClient;
        _configuracaoPagamentoMercadoPagoRepository = configuracaoPagamentoMercadoPagoRepository;
    }

    public IPagamento Get(TipoDePagamento tipoDePagamento)
    {
        switch (tipoDePagamento)
        {
            default:
                return new PagamentoViaPix(_mercadoPagoHttpClient, _configuracaoPagamentoMercadoPagoRepository);
        }
    }
}
