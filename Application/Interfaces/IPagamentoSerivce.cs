using Application.Dtos.MercadoPago;
using Application.Dtos.Pagamentos;
using Domain.Model;

namespace Application.Interfaces;

public interface IPagamentoSerivce
{
    Task<ResultPagamento> EfetuarPagamentoAsync(EfetuarPagamentoDto efetuarPagamentoDto, string referer);
    Task AtualizarPagamento(dynamic mercadoPagoWebHook, string cliente);
}
