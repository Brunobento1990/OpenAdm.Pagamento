using Application.Interfaces;
using Application.Models.Usuarios;
using Domain.Pkg.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services;

public sealed class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UsuarioViewModel GetTokenUsuarioViewModel()
    {
        if (_httpContextAccessor?.HttpContext?.User.Identity is not ClaimsIdentity claimsIdentity
            || !claimsIdentity.Claims.Any())
            throw new ExceptionApi(nameof(claimsIdentity));

        var id = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value
            ?? throw new ExceptionApi("token inválido");
        var numero = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Numero")?.Value
            ?? throw new ExceptionApi("token inválido");
        var nome = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Nome")?.Value
            ?? throw new ExceptionApi("token inválido");
        var email = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value
            ?? throw new ExceptionApi("token inválido");

        var dataDeCriacao = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "DataDeCriacao")?.Value
            ?? throw new ExceptionApi("token inválido");

        var dataDeAtualizacao = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "DataDeAtualizacao")?.Value
            ?? throw new ExceptionApi("token inválido");

        var telefone = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Telefone")?.Value;
        var cnpj = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
        var cpf = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Cpf")?.Value;

        if (!DateTime.TryParse(dataDeCriacao, out DateTime newDataDeCriacao))
            throw new ExceptionApi("token inválido");

        if (!DateTime.TryParse(dataDeAtualizacao, out DateTime newDataDeAtualizacao))
            throw new ExceptionApi("token inválido");

        return new UsuarioViewModel()
        {
            Id = Guid.Parse(id),
            Nome = nome,
            Email = email,
            Numero = long.Parse(numero),
            DataDeCriacao = newDataDeCriacao,
            DataDeAtualizacao = newDataDeAtualizacao,
            Telefone = telefone,
            Cnpj = cnpj,
            Cpf = cpf
        };
    }
}
