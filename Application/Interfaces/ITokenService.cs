using Application.Models.Usuarios;

namespace Application.Interfaces;

public interface ITokenService
{
    UsuarioViewModel GetTokenUsuarioViewModel();
}
