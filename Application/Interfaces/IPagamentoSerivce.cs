using Application.Dtos.Pagamentos;
using Domain.Model;

namespace Application.Interfaces;

public interface IPagamentoSerivce
{
    Task<ResultPagamento> EfetuarPagamentoAsync(EfetuarPagamentoDto efetuarPagamentoDto);
}
