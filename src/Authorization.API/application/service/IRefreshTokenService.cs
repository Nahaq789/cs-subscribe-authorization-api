namespace Authorization.API.application.service;

public interface IRefreshTokenService
{
    string GenerateRefreshToken();
    Task<string> ValidateRefreshToken(string refreshToken, CancellationToken cancellationToken);
    Task AddRefreshToken(string userId, string refreshToken, DateTime expiresIn);
    Task RemoveRefreshToken(string refreshToken, CancellationToken cancellationToken);
}