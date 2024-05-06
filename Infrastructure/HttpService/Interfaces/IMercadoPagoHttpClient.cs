using Domain.Model;
using Infrastructure.Model.MercadoPago;

namespace Infrastructure.HttpService.Interfaces;

public interface IMercadoPagoHttpClient
{
    Task<MercadoPagoResponse> PostAsync(MercadoPagoRequest mercadoPagoRequest, string accessToken);
}
