using Authorization.API.domain.dto;

namespace Authorization.API.infrastructure.repository;

public interface IUserAuthRepository
{
    Task<UserAuth> GetByEmailAndPass(string email, string pass);
    Task<UserAuth> GetByEmail(string email);
}