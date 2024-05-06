namespace Domain.Model;

public class ResultPagamento
{
    public string? QrCodePix { get; set; }
    public string? QrCodePixBase64 { get; set; }
    public string? LinkPagamento { get; set; }
    public int MercadoPagoId { get; set; }
}
