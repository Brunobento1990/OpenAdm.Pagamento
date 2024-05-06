namespace Application.Dtos.MercadoPago;

public class MercadoPagoWebHook
{
    public string? Action { get; set; } 
    public string? Id { get; set; }
    public DataMercadoPago? Data { get; set; }
}
