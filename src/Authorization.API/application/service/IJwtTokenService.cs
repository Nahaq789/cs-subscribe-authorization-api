using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
namespace Authorization.API.application.service;

public interface IJwtTokenService
{
    JwtSecurityToken GenerateNewToken(IEnumerable<Claim> claims, DateTime expires);
}