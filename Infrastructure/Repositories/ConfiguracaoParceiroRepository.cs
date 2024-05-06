using Domain.Interfaces;
using Domain.Pkg.Cryptography;
using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class ConfiguracaoParceiroRepository
    : IConfiguracaoParceiroRepository
{
    private readonly OpenAdmContext _context;
    private readonly string _dominio;

    public ConfiguracaoParceiroRepository(
        OpenAdmContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _dominio = httpContextAccessor?
           .HttpContext?
           .Request?
           .Headers?
           .FirstOrDefault(x => x.Key == "Referer").Value.ToString() ?? throw new Exception();
    }

    public async Task<ConfiguracaoParceiro?> GetByDomainAsync(string domain)
    {
        return await _context
            .ConfiguracoesParceiro
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ativo && (x.DominioSiteAdm == domain || x.DominioSiteEcommerce == domain));
    }

    public async Task<string> GetConexaoDbByDominioAsync()
    {
        var encrypt = await _context
            .ConfiguracoesParceiro
            .AsNoTracking()
            .Where(x => x.Ativo && (x.DominioSiteAdm == _dominio || x.DominioSiteEcommerce == _dominio))
            .Select(x => x.ConexaoDb)
            .FirstOrDefaultAsync()
                ?? throw new ExceptionApi();

        return CryptographyGeneric.Decrypt(encrypt);
    }
}
