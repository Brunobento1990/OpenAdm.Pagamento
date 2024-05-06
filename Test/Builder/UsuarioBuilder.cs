using Domain.Pkg.Entities;

namespace Test.Builder;

public class UsuarioBuilder
{
    public static UsuarioBuilder Init() => new();

    public Usuario Build()
    {
        return new Usuario(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now,
            1,
            "teste@gmail.com",
            "123",
            "teste",
            "123",
            "132",
            "123",
            true);
    }
}
