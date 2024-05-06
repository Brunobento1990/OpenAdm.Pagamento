using Domain.Enuns;

namespace Application.Dtos.Pagamentos;

public class EfetuarPagamentoDto
{
    public TipoDePagamento TipoDePagamento { get; set; }
    public Guid PedidoId { get; set; }
}
