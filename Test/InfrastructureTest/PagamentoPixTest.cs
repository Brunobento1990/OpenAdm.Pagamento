using Domain.Entities;
using Domain.Interfaces;
using Domain.Pkg.Cryptography;
using Infrastructure.HttpService.Interfaces;
using Infrastructure.Model.MercadoPago;
using Infrastructure.Pagamento.Services;
using Test.Builder;

namespace Test.InfrastructureTest;

public class PagamentoPixTest
{

    public PagamentoPixTest()
    {
        CryptographyGeneric.Configure("f1c2bd7945f04ea7b9adcee2290100c5", "f1c2bd7945f04ea8");
    }

    private readonly Mock<IMercadoPagoHttpClient> _mercadoPagoHttpClientMock = new();
    private readonly Mock<IConfiguracaoPagamentoMercadoPagoRepository> _configuracaoPagamentoRepositoryMock = new();
    [Fact]
    public async Task NaoDevePagarSemConfiguracaoDePagamento()
    {
        var mercadoPagoRequest = MercadoPagoRequestBuilder.Init().Build();
        _configuracaoPagamentoRepositoryMock.Setup(x => x.GetAsync())
            .ReturnsAsync((ConfiguracaoPagamentoMercadoPago?)null);
        var pagamentoPix = new PagamentoViaPix(
            _mercadoPagoHttpClientMock.Object,
            _configuracaoPagamentoRepositoryMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => 
            await pagamentoPix.PagarAsync(mercadoPagoRequest));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDevePagarSemAcessToken(string accessToken)
    {
        var mercadoPagoRequest = MercadoPagoRequestBuilder.Init().Build();
        var config = ConfiguracaoPagamentoMercadoPagoBuilder
            .Init()
            .Builder();

        var responseMercadoPago = new MercadoPagoResponse();

        _configuracaoPagamentoRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(config);
        _mercadoPagoHttpClientMock.Setup(x => 
            x.PostAsync(mercadoPagoRequest, accessToken))
            .ReturnsAsync(responseMercadoPago);

        var pagamentoPix = new PagamentoViaPix(
            _mercadoPagoHttpClientMock.Object,
            _configuracaoPagamentoRepositoryMock.Object);

        await Assert.ThrowsAsync<Exception>(async () =>
            await pagamentoPix.PagarAsync(mercadoPagoRequest));
    }

    [Fact]
    public async Task DeveFazerPagamento()
    {
        var mercadoPagoRequest = MercadoPagoRequestBuilder.Init().Build();
        var config = ConfiguracaoPagamentoMercadoPagoBuilder
            .Init()
            .AddAccessToken(CryptographyGeneric.Encrypt("555"))
            .Builder();

        var mercadoPagoResponse = new MercadoPagoResponse();

        _configuracaoPagamentoRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(config);
        _mercadoPagoHttpClientMock.Setup(x =>
            x.PostAsync(mercadoPagoRequest, CryptographyGeneric.Decrypt(config.AccessToken)))
            .ReturnsAsync(mercadoPagoResponse);

        var pagamentoPix = new PagamentoViaPix(
            _mercadoPagoHttpClientMock.Object,
            _configuracaoPagamentoRepositoryMock.Object);

        var response = await pagamentoPix.PagarAsync(mercadoPagoRequest);

        Assert.NotNull(response);
    }
}
