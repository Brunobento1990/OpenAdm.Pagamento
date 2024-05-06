using Domain.Pkg.Entities;

namespace Application.Models.Usuarios;

public class UsuarioViewModel : BaseViewModel
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }

    public UsuarioViewModel ToModel(Usuario entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Email = entity.Email;
        Numero = entity.Numero;
        Telefone = entity.Telefone;
        Nome = entity.Nome;
        Cnpj = entity.Cnpj;
        Cpf = entity.Cpf;

        return this;
    }
}
