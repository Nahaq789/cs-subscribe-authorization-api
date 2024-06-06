using System.IdentityModel.Tokens.Jwt;
using Authorization.API.application.service;
using Authorization.API.domain.exception;
using Authorization.API.infrastructure.repository;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.API.application.command;

public class JwtTokenGenerateWhenTokenExpiredCommandHandler : IRequestHandler<JwtTokenGenerateWhenTokenExpiredCommand, JwtSecurityToken>
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public JwtTokenGenerateWhenTokenExpiredCommandHandler(IUserAuthRepository userAuthRepository, IJwtTokenService jwtTokenService, IRefreshTokenService refreshTokenService)
    {
        this._userAuthRepository = userAuthRepository;
        this._jwtTokenService = jwtTokenService;
        this._refreshTokenService = refreshTokenService;
    }

    public async Task<JwtSecurityToken> Handle(JwtTokenGenerateWhenTokenExpiredCommand command, CancellationToken cancellationToken)
    {
        var refValid = await _refreshTokenService.ValidateRefreshToken(command.UserId, command.RefreshToken, cancellationToken);
        if (!refValid) throw new SecurityTokenExpiredException("リフレッシュトークンの有効期限が切れています。");

        var user = await _userAuthRepository.GetByEmail(command.Email);
        if (user == null) throw new UserDomainException("ユーザーが見つかりませんでした。");
        return _jwtTokenService.GenerateJwtToken(command.Email);
    }
}