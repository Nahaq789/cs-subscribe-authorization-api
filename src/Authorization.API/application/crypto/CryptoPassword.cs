using System.Security.Cryptography;
using System.Text;

namespace Authorization.API.application.crypto;

public static class CryptoPassword
{
    /// <summary>
    /// パスワードをソルトとストレッチングで暗号化する
    /// </summary>
    /// <param name="passwordWithSalt">パスワード+ソルト</param>
    /// <returns>暗号化された文字列</returns>
    public static string HashPassword(string passwordWithSalt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.Append(bytes[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }

    /// <summary>
    /// ランダムなソルトを生成
    /// </summary>
    /// <returns>ランダムなソルト</returns>
    public static string SaltPassword()
    {
        byte[] bytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// パスワード+ソルトを一定回数ハッシュ化を行う。
    /// </summary>
    /// <param name="password">ユーザ入力したパスワード</param>
    /// <param name="salt">ランダムに生成されたソルト</param>
    /// <param name="iter">ハッシュ化を行う回数</param>
    /// <returns>DBに保存される暗号化されたパスワード</returns>
    public static string HashPasswordWithStretching(string password, string salt, int iter = 1000)
    {
        var hash = HashPassword(password + salt);
        for (int i = 0; i < iter; i++)
        {
            hash = HashPassword(hash);
        }

        return hash;
    }
}