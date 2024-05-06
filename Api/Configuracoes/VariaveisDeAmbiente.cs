using Domain.Pkg.Exceptions;

namespace Api.Configuracoes;

public static class VariaveisDeAmbiente
{
    public static string GetVariavel(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ExceptionApi($"Key inválida : {key}");

        return Environment.GetEnvironmentVariable(key) ??
            throw new ExceptionApi($"Variável não encontrada com a Key : {key}");
    }
}