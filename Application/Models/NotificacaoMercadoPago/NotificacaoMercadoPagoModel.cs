using Domain.Interfaces;

namespace Application.Models.NotificacaoMercadoPago;

public class NotificacaoMercadoPagoModel : INotificacaoMercadoPagoModel
{
    public string ConnectionString { get; set; } = string.Empty;
}
