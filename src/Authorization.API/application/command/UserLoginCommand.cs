using System.Runtime.Serialization;
using Authorization.API.application.model;
using MediatR;

namespace Authorization.API.application.command;

[DataContract]
public class UserLoginCommand : IRequest<AuthResult>
{
    [DataMember]
    public string Email { get; private set; }
    [DataMember]
    public string Password { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="email">メールアドレス</param>
    /// <param name="password">パスワード</param>
    public UserLoginCommand(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
}