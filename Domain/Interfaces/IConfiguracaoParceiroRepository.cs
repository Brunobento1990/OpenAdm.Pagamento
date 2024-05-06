using Domain.Pkg.Entities;

namespace Domain.Interfaces;

public interface IConfiguracaoParceiroRepository
{
    Task<string> GetConexaoDbByDominioAsync();
    Task<ConfiguracaoParceiro?> GetByDomainAsync(string domain);
}
