using System.IdentityModel.Tokens.Jwt;
using Authorization.API.application.model;
using MediatR;

namespace Authorization.API.application.command;

public class JwtTokenGenerateWhenTokenExpiredCommand : IRequest<AuthResult>
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