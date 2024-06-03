using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authorization.API.application.dto;
using Authorization.API.application.service;
using Authorization.API.infrastructure.repository;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Authorization.API.application.command;

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, LoginResult>
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly ICryptoPasswordService _cryptoPasswordService;
    private readonly IJwtTokenService _jwtTokenService;

    /// <param name="userAuthRepository">ユーザーオースリポジトリ</param>
    /// <param name="configuration">コンフィグ</param>
    public UserLoginCommandHandler(
        IUserAuthRepository userAuthRepository,
        ICryptoPasswordService cryptoPasswordService,
        IJwtTokenService jwtTokenService)
    {
        this._userAuthRepository = userAuthRepository ?? throw new ArgumentNullException(nameof(userAuthRepository));
        this._cryptoPasswordService = cryptoPasswordService ?? throw new ArgumentNullException(nameof(cryptoPasswordService));
        this._jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
    }

    /// <summary>
    /// コマンドハンドラーです。暗号化されたトークンを返します。
    /// </summary>
    /// <param name="email">ユーザーログインコマンド</param>
    /// <param name="password">キャンセレーショントークン</param>
    public async Task<LoginResult> Handle(UserLoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userAuthRepository.GetByEmail(command.Email);
        if (user == null)
        {
            return new LoginResult
            {
                Success = false,
                ErrorMessage = "ユーザーが見つかりません。"
            };
        }

        var hashPassword = _cryptoPasswordService.HashPassword(command.Password, user.Salt);
        var target = await _userAuthRepository.GetByEmailAndPass(command.Email, hashPassword);
        if (target == null)
        {
            return new LoginResult
            {
                Success = false,
                ErrorMessage = "ログイン情報が無効です。"
            };
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, command.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString())
        };
        var token = _jwtTokenService.GenerateNewToken(claims, DateTime.Now.AddMinutes(30));
        return new LoginResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}