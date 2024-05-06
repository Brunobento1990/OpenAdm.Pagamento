using Application.Dtos.Pagamentos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

public sealed class PagamentoSerivce : IPagamentoSerivce
{
    private readonly IPagamenoFactory _pagamenoFactory;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPagamentoPedidoRepository _pagamentoPedidoRepository;

    public PagamentoSerivce(
        IPagamenoFactory pagamenoFactory,
        IPedidoRepository pedidoRepository,
        IPagamentoPedidoRepository pagamentoPedidoRepository)
    {
        _pagamenoFactory = pagamenoFactory;
        _pedidoRepository = pedidoRepository;
        _pagamentoPedidoRepository = pagamentoPedidoRepository;
    }

    public async Task<ResultPagamento> EfetuarPagamentoAsync(EfetuarPagamentoDto efetuarPagamentoDto)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(efetuarPagamentoDto.PedidoId)
            ?? throw new Exception($"O pedido não foi localizado, ID: {efetuarPagamentoDto.PedidoId}");

        var factory = _pagamenoFactory.Get(efetuarPagamentoDto.TipoDePagamento);

        var mercadoPagoRequest = new MercadoPagoRequest()
        {
            Description = $"Pedido {pedido.Numero}",
            Transaction_amount = pedido.ValorTotal,
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
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            qrCodePix: resultPagamento.QrCodePix,
            qrCodePixBase64: resultPagamento.QrCodePixBase64,
            linkPagamento: resultPagamento.LinkPagamento,
            mercadoPagoId: resultPagamento.MercadoPagoId,
            pedidoId: pedido.Id,
            false);

        await _pagamentoPedidoRepository.AddAsync(pagamentoPedido);

        return resultPagamento;
    }
}
