using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql.Replication;
using System;
using System.Security.Cryptography;

namespace Authorization.API.application.service;

public class CryptoPasswordService : ICryptoPasswordService
{
    private readonly IConfiguration _configuration;

    public CryptoPasswordService(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    private int IterationCount => _configuration.GetValue<int>("PasswordHashSettings:IterationCount");
    private int NumBytesRequested => _configuration.GetValue<int>("PasswordHashSettings:NumBytesRequested");

    /// <summary>
    /// パスワードをPbkdf2で暗号化する
    /// </summary>
    /// <param name="password">パスワード</param>
    /// <param name="salt">ソルト</param>
    /// <returns>暗号化された文字列</returns>
    public string HashPassword(string password, string salt)
    {
        byte[] salts = Convert.FromBase64String(salt);
        var hash = KeyDerivation.Pbkdf2(
            password,
            salts,
            KeyDerivationPrf.HMACSHA256,
            IterationCount,
            NumBytesRequested
        );
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// ランダムなソルトを生成
    /// </summary>
    /// <returns>ランダムなソルト</returns>
    public string CreateSalt()
    {
        byte[] bytes = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        return Convert.ToBase64String(bytes);
    }

}