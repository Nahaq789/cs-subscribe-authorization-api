using Authorization.API.application.crypto;
using Authorization.API.application.dto;
using Authorization.API.infrastructure.repository;
using MediatR;

namespace Authorization.API.application.command;

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, LoginResult>
{
    private readonly IUserAuthRepository _userAuthRepository;

    /// <param name="userAuthRepository">ユーザーオースリポジトリ</param>
    public UserLoginCommandHandler(IUserAuthRepository userAuthRepository)
    {
        this._userAuthRepository = userAuthRepository ?? throw new ArgumentNullException(nameof(userAuthRepository));
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
        return new LoginResult
        {
            Success = true,
            Token = ""
        };
    }
}