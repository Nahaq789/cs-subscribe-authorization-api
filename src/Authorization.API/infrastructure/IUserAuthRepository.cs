using Authorization.API.domain;

namespace Authorization.API.infrastructure;

public interface IUserAuthRepository
{
    Task<UserAuth> GetByEmailAndPass(string email, string pass);
}