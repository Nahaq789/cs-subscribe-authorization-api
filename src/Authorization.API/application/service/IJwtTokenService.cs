using System.IdentityModel.Tokens.Jwt;
namespace Authorization.API.application.service;

/// <summary>
/// JWTトークンを生成するサービスです。
/// </summary>
public interface IJwtTokenService
{

    /// <summary>
    /// ユーザーのメールアドレスを含めてトークンを生成
    /// </summary>
    /// <param name="email">メールアドレス</param>
    JwtSecurityToken GenerateJwtToken(string email);

    /// <summary>
    /// JWTトークンを復元し認証
    /// </summary>
    /// <param name="token">jwtトークン</param>
    bool ValidateJwtToken(string token);
}