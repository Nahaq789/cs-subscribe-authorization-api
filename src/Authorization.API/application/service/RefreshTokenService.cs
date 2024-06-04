using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Authorization.API.application.service;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IDistributedCache _cache;

    public RefreshTokenService(IDistributedCache cache)
    {
        this._cache = cache;
    }
    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
    public async Task<string> ValidateRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _cache.GetStringAsync(refreshToken, cancellationToken);
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task AddRefreshToken(string userId, string refreshToken, DateTime expiresIn)
    {
        var token = new RefreshToken(refreshToken, expiresIn);

        var serializedToken = JsonSerializer.Serialize(token);
        await _cache.SetStringAsync(userId, serializedToken);
    }

    public async Task RemoveRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(refreshToken, cancellationToken);
    }

}

public record RefreshToken(
    string token, DateTime expiration
);