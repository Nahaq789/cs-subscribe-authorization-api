using Authorization.API.domain;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.infrastructure.repositroy;

public class UserAuthRepository : IUserAuthRepository
{
    private readonly UserContext _context;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="contexrt">ユーザーコンテキスト</param>
    public UserAuthRepository(UserContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// EmailとPasswordが一致するユーザーを取得します。
    /// </summary>
    public async Task<UserAuth> GetByEmailAndPass(string email, string password)
    {
        var result = await _context.UserAuth.FromSql(
            $"SELECT Email, Password FROM User WHERE Email = {email} AND Password = {password}"
        ).FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
        return result;
#pragma warning restore CS8603 // Possible null reference return.
    }

}