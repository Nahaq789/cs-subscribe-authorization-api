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
    public async Task<bool> ValidateRefreshToken(string userId, string refreshToken, CancellationToken cancellationToken)
    {
        var token = await _cache.GetStringAsync(userId, cancellationToken);
        if (string.IsNullOrEmpty(token)) return false;

        var storedToken = JsonSerializer.Deserialize<RefreshToken>(token);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return storedToken.token == refreshToken && storedToken.expiration == DateTime.UtcNow;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

    }

    public async Task AddRefreshToken(string userId, string refreshToken, DateTime expiresIn)
    {
        var token = new RefreshToken(refreshToken, expiresIn);

        var serializedToken = JsonSerializer.Serialize(token);
        await _cache.SetStringAsync(userId, serializedToken);
    }

    public async Task RemoveRefreshToken(string userId, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(userId, cancellationToken);
    }

}

public record RefreshToken(
    string token, DateTime expiration
);