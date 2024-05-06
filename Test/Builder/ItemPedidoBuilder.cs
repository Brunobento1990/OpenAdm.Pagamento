using Domain.Pkg.Entities;

namespace Test.Builder;

public class ItemPedidoBuilder
{
    public static ItemPedidoBuilder Init() => new();

    public ItensPedido Build()
    {
        return new ItensPedido(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now,
            1,
            null,
            null,
            Guid.NewGuid(),
            Guid.NewGuid(),
            1,
            2);
    }
}
