using System.IdentityModel.Tokens.Jwt;
using MediatR;

namespace Authorization.API.application.command;

public class JwtTokenGenerateWhenTokenExpiredCommand : IRequest<JwtSecurityToken>
{
    public string UserId { get; private set; }
    public string Email { get; private set; }
    public string RefreshToken { get; private set; }

    public JwtTokenGenerateWhenTokenExpiredCommand(string userId, string email, string refreshToken)
    {
        this.UserId = userId;
        this.Email = email;
        this.RefreshToken = refreshToken;
    }
}