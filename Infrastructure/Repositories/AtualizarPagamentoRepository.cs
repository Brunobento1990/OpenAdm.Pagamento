using Domain.Interfaces;
using Domain.Pkg.Cryptography;
using Domain.Pkg.Enum;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class AtualizarPagamentoRepository : IAtualizarPagamentoRepository
{
    private readonly OpenAdmContext _openAdmContext;
    private readonly INotificacaoMercadoPagoModel _notificacaoMercadoPagoModel;

    public AtualizarPagamentoRepository(
        OpenAdmContext openAdmContext, 
        INotificacaoMercadoPagoModel notificacaoMercadoPagoModel)
    {
        _openAdmContext = openAdmContext;
        _notificacaoMercadoPagoModel = notificacaoMercadoPagoModel;
    }

    public async Task<bool> AtualizarAsync(long mercadoPagoId, string cliente)
    {
        var optionsBuilderParceiro = new DbContextOptionsBuilder<ContextMercadoPago>();

        optionsBuilderParceiro.UseNpgsql(_notificacaoMercadoPagoModel.ConnectionString);

        var mercadoPagoContext = new ContextMercadoPago(optionsBuilderParceiro.Options);

        var pagamento = await mercadoPagoContext
            .PagamentosPedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MercadoPagoId == mercadoPagoId);

        if (pagamento == null || pagamento.Pago) return false;

        var pedido = await mercadoPagoContext
            .Pedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == pagamento.PedidoId);

        if (pedido == null || pedido.StatusPedido != StatusPedido.Aberto) return false;

        pagamento.UpdatePagamento();
        pedido.UpdateStatus(StatusPedido.Faturado);

        mercadoPagoContext.Attach(pedido);
        mercadoPagoContext.Attach(pagamento);

        mercadoPagoContext.Update(pedido);
        mercadoPagoContext.Update(pagamento);

        await mercadoPagoContext.SaveChangesAsync();

        return true;
    }
}
