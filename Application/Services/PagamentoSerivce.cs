using Application.Dtos.MercadoPago;
using Application.Dtos.Pagamentos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

public sealed class PagamentoSerivce : IPagamentoSerivce
{
    private readonly IAtualizarPagamentoRepository _atualizarPagamentoRepository;
    private readonly IPagamenoFactory _pagamenoFactory;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPagamentoPedidoRepository _pagamentoPedidoRepository;
    private readonly IConfiguracaoParceiroRepository _configuracaoParceiroRepository;

    public PagamentoSerivce(
        IPagamenoFactory pagamenoFactory,
        IPedidoRepository pedidoRepository,
        IPagamentoPedidoRepository pagamentoPedidoRepository,
        IAtualizarPagamentoRepository atualizarPagamentoRepository,
        IConfiguracaoParceiroRepository configuracaoParceiroRepository)
    {
        _pagamenoFactory = pagamenoFactory;
        _pedidoRepository = pedidoRepository;
        _pagamentoPedidoRepository = pagamentoPedidoRepository;
        _atualizarPagamentoRepository = atualizarPagamentoRepository;
        _configuracaoParceiroRepository = configuracaoParceiroRepository;
    }

    public async Task AtualizarPagamento(MercadoPagoWebHook mercadoPagoWebHook, string cliente)
    {
        await _atualizarPagamentoRepository.AtualizarAsync(mercadoPagoWebHook.Data?.Id ?? 0, cliente);
    }

    public async Task<ResultPagamento> EfetuarPagamentoAsync(EfetuarPagamentoDto efetuarPagamentoDto, string referer)
    {
        var configuracaoParceiro = await _configuracaoParceiroRepository.GetByDomainAsync(referer);
        var pedido = await _pedidoRepository.GetByIdAsync(efetuarPagamentoDto.PedidoId)
            ?? throw new Exception($"O pedido não foi localizado, ID: {efetuarPagamentoDto.PedidoId}");

        var factory = _pagamenoFactory.Get(efetuarPagamentoDto.TipoDePagamento);
        var payment_id = Guid.NewGuid();

        var mercadoPagoRequest = new MercadoPagoRequest()
        {
            Description = $"Pedido {pedido.Numero}",
            Transaction_amount = pedido.ValorTotal,
            External_reference = payment_id.ToString(),
            Notification_url = $"https://api.open-adm.tech/api/v1/pagamento/pagamento/notificar?cliente={configuracaoParceiro?.ClienteMercadoPago ?? ""}",
            Payer = new()
            {
                Email = pedido.Usuario.Email,
                First_name = pedido.Usuario.Nome,
                Identification = new()
                {
                    Type = string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj) ? "CPF" : "CNPJ",
                    Number = string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj) ? pedido.Usuario.Cpf ?? "" : pedido.Usuario.Cnpj ?? ""
                }
            }
        };

        var resultPagamento = await factory.PagarAsync(mercadoPagoRequest);

        var pagamentoPedido = new PagamentoPedido(
            id: payment_id,
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            qrCodePix: resultPagamento.QrCodePix,
            qrCodePixBase64: resultPagamento.QrCodePixBase64,
            linkPagamento: resultPagamento.LinkPagamento,
            mercadoPagoId: resultPagamento.MercadoPagoId ?? 0,
            pedidoId: pedido.Id,
            false);

        await _pagamentoPedidoRepository.AddAsync(pagamentoPedido);

        resultPagamento.MercadoPagoId = null;

        return resultPagamento;
    }
}
