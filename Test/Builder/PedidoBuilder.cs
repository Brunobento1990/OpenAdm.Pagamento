using Domain.Pkg.Entities;

namespace Test.Builder;

public class PedidoBuilder
{
    public static PedidoBuilder Init() => new();

    public Pedido Build()
    {
        return new Pedido(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now,
            1,
            Domain.Pkg.Enum.StatusPedido.Faturado,
            Guid.NewGuid());
    }
}
