namespace Authorization.API.application.service;

/// <summary>
/// リフレッシュトークンをRedisに保存します。
/// </summary>
public interface IRefreshTokenService
{
    /// <summary>
    /// ランダムなリフレッシュトークンを生成します。
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Redisからリフレッシュトークンを取得します。
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    Task<string> ValidateRefreshToken(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// リフレッシュトークンをRedisに追加します。
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="refreshToken">リフレッシュトークン</param>
    Task AddRefreshToken(string userId, string refreshToken, DateTime expiresIn);

    /// <summary>
    /// リフレッシュトークンをRedisから削除します。
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    Task RemoveRefreshToken(string userId, CancellationToken cancellationToken);
}