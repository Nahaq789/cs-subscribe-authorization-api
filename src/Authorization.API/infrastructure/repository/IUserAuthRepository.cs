using Authorization.API.domain;

namespace Authorization.API.infrastructure.repository;

public interface IUserAuthRepository
{
    Task<UserAuth> GetByEmailAndPass(string email, string pass);
}