using Authorization.API.domain;
using Authorization.API.domain.dto;
using Authorization.API.infrastructure.repository;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.infrastructure.repository;

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
        var result = await (from userAggregate in _context.Set<UserAggregate>()
                            join user in _context.Set<UserEntity>()
                                on userAggregate.AggregateId equals user.AggregateId
                            join salt in _context.Set<UserSalt>()
                                on userAggregate.AggregateId equals salt.AggregateId
                            where user.Email == email && user.Password == password
                            select new UserAuth(userAggregate.AggregateId, user.Email, user.Password, salt.Salt))
                     .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
        return result;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task<UserAuth> GetByEmail(string email)
    {
        var result = await (from userAggregate in _context.Set<UserAggregate>()
                            join user in _context.Set<UserEntity>()
                                on userAggregate.AggregateId equals user.AggregateId
                            join salt in _context.Set<UserSalt>()
                                on userAggregate.AggregateId equals salt.AggregateId
                            where user.Email == email
                            select new UserAuth(userAggregate.AggregateId, user.Email, user.Password, salt.Salt))
                     .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
        return result;
#pragma warning restore CS8603 // Possible null reference return.
    }
}