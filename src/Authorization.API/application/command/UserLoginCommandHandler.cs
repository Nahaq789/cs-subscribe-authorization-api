using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authorization.API.application.crypto;
using Authorization.API.application.dto;
using Authorization.API.infrastructure.repository;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Authorization.API.application.command;

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, LoginResult>
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly IConfiguration _configuration;

    /// <param name="userAuthRepository">ユーザーオースリポジトリ</param>
    /// <param name="configuration">コンフィグ</param>
    public UserLoginCommandHandler(IUserAuthRepository userAuthRepository, IConfiguration configuration)
    {
        this._userAuthRepository = userAuthRepository ?? throw new ArgumentNullException(nameof(userAuthRepository));
        this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// コマンドハンドラーです。暗号化されたトークンを返します。
    /// </summary>
    /// <param name="email">ユーザーログインコマンド</param>
    /// <param name="password">キャンセレーショントークン</param>
    public async Task<LoginResult> Handle(UserLoginCommand command, CancellationToken cancellationToken)
    {
        var salt = CryptoPassword.SaltPassword();
        var hashPassword = CryptoPassword.HashPasswordWithStretching(command.Password, salt);
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
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt_Key") ?? string.Empty));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("Jwt:Issuer"),
            audience: _configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new LoginResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}