using Authorization.API.application.crypto;
using Authorization.API.application.exceptions;
using Authorization.API.infrastructure;
using MediatR;

namespace Authorization.API.application.command;

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, string>
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
    public async Task<string> Handle(UserLoginCommand command, CancellationToken cancellationToken)
    {
        var decryptPassword = CryptoPassword.Encrypto(command.Password);
        var target = await _userAuthRepository.GetByEmailAndPass(command.Email, command.Password);
        if (target == null)
        {
            return "メールアドレスまたはパスワードが間違っています。";
        }
        return "ログインに成功しました。";
    }
}