using Domain.Model;

namespace Test.Builder;

public class MercadoPagoRequestBuilder
{
    private decimal _transaction_amount { get; set; }
    private string _description { get; set; } = string.Empty;
    private string _payment_method_id { get; set; } = "pix";

    public static MercadoPagoRequestBuilder Init() => new MercadoPagoRequestBuilder();

    public MercadoPagoRequestBuilder PaymentMethod(string paymentMethod)
    {
        _payment_method_id = paymentMethod;
        return this;
    }

    public MercadoPagoRequestBuilder Description(string description)
    {
        _description = description;
        return this;
    }

    public MercadoPagoRequestBuilder TransactionAmount(decimal transaction_amount)
    {
        _transaction_amount = transaction_amount;
        return this;
    }

    public MercadoPagoRequest Build()
    {
        return new MercadoPagoRequest() 
        {
            Transaction_amount = _transaction_amount,
            Description = _description,
            Payment_method_id = _payment_method_id
        };
    }
}
