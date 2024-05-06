using Application.Dtos.MercadoPago;
using Application.Dtos.Pagamentos;
using Domain.Model;

namespace Application.Interfaces;

public interface IPagamentoSerivce
{
    Task<ResultPagamento> EfetuarPagamentoAsync(EfetuarPagamentoDto efetuarPagamentoDto);
    Task AtualizarPagamento(MercadoPagoWebHook mercadoPagoWebHook);
}
