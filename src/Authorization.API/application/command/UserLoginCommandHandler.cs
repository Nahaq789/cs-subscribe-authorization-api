using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authorization.API.application.dto;
using Authorization.API.application.service;
using Authorization.API.infrastructure.repository;
using MediatR;

namespace Authorization.API.application.command;

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, LoginResult>
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly ICryptoPasswordService _cryptoPasswordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    /// <param name="userAuthRepository">ユーザーオースリポジトリ</param>
    /// <param name="configuration">コンフィグ</param>
    public UserLoginCommandHandler(
        IUserAuthRepository userAuthRepository,
        ICryptoPasswordService cryptoPasswordService,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService)
    {
        this._userAuthRepository = userAuthRepository ?? throw new ArgumentNullException(nameof(userAuthRepository));
        this._cryptoPasswordService = cryptoPasswordService ?? throw new ArgumentNullException(nameof(cryptoPasswordService));
        this._jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        this._refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
    }

    /// <summary>
    /// コマンドハンドラーです。暗号化されたトークンを返します。
    /// </summary>
    /// <param name="command">ユーザーログインコマンド</param>
    public async Task<LoginResult> Handle(UserLoginCommand command, CancellationToken cancellationToken)
    {
        // メールアドレスのみで取得
        var user = await _userAuthRepository.GetByEmail(command.Email);
        if (user == null)
        {
            return new LoginResult
            {
                Success = false,
                ErrorMessage = "ユーザーが見つかりません。"
            };
        }

        // メールアドレスとハッシュ化されたパスワードで取得
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
        var token = _jwtTokenService.GenerateJwtToken(command.Email);

        var refreshToken = _refreshTokenService.GenerateRefreshToken();
        // Redisにリフレッシュトークンを保存する
        await _refreshTokenService.AddRefreshToken(target.UserId.ToString(), refreshToken, DateTime.Now.AddDays(7));
        return new LoginResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
        };
    }
}