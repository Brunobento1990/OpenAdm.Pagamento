using Domain.Model;

namespace Domain.Interfaces;

public interface IPagamento
{
    Task<ResultPagamento> PagarAsync(MercadoPagoRequest mercadoPagoRequest);
}
