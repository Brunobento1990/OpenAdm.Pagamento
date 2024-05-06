using Application.Dtos.ConfiguracoesPagamentosMercadoPago;
using Application.Interfaces;
using Application.Models.ConfiguracoesPagamentosMercadoPago;
using Domain.Interfaces;
using Domain.Pkg.Cryptography;

namespace Application.Services;

public sealed class ConfiguracoesPagamentosMercadoPagoService : IConfiguracoesPagamentosMercadoPagoService
{
    private readonly IConfiguracaoPagamentoMercadoPagoRepository
        _configuracaoPagamentoMercadoPagoRepository;
    private readonly ITokenService _tokenService;

    public ConfiguracoesPagamentosMercadoPagoService(
        IConfiguracaoPagamentoMercadoPagoRepository configuracaoPagamentoMercadoPagoRepository,
        ITokenService tokenService)
    {
        _configuracaoPagamentoMercadoPagoRepository = configuracaoPagamentoMercadoPagoRepository;
        _tokenService = tokenService;
    }

    public async Task<bool> CobrarAsync()
    {
        var config = await _configuracaoPagamentoMercadoPagoRepository.GetAsync();
        if (config == null) return false;

        var usuario = _tokenService.GetTokenUsuarioViewModel();

        if (!string.IsNullOrWhiteSpace(usuario.Cnpj))
        {
            return config.CobrarCnpj.HasValue;
        }

        if (!string.IsNullOrWhiteSpace(usuario.Cpf))
        {
            return config.CobrarCpf.HasValue;
        }

        return false;
    }

    public async Task<ConfiguracaoPagamentoMercadoPagoViewModel> CreateOrUpdateAsync(
        CreateConfiguracoesPagamentosMercadoPagoDto dto)
    {
        var config = await _configuracaoPagamentoMercadoPagoRepository.GetAsync();

        if (config == null)
        {
            config = dto.ToEntity();
            await _configuracaoPagamentoMercadoPagoRepository.AddAsync(config);
        }
        else
        {
            var publicKeyCrypt = CryptographyGeneric.Encrypt(dto.PublicKey);
            var accessTokenCrypt = CryptographyGeneric.Encrypt(dto.AccessToken);
            config.Update(publicKeyCrypt, accessTokenCrypt, dto.CobrarCpf, dto.CobrarCnpj);
            await _configuracaoPagamentoMercadoPagoRepository.UpdateAsync(config);
        }

        return new ConfiguracaoPagamentoMercadoPagoViewModel().ToModel(config);
    }

    public async Task<ConfiguracaoPagamentoMercadoPagoViewModel> GetAsync()
    {
        var config = await _configuracaoPagamentoMercadoPagoRepository.GetAsync();

        if (config == null) return new ConfiguracaoPagamentoMercadoPagoViewModel();

        return new ConfiguracaoPagamentoMercadoPagoViewModel().ToModel(config);
    }
}
