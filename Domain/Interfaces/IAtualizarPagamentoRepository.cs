namespace Domain.Interfaces;

public interface IAtualizarPagamentoRepository
{
    Task<bool> AtualizarAsync(long mercadoPagoId, string cliente);
}
