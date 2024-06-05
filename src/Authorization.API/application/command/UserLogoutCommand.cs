using System.Runtime.Serialization;
using MediatR;

namespace Authorization.API.application.command;

[DataContract]
public class UserLogoutCommand : IRequest<string>
{
    [DataMember]
    public string UserId { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    public UserLogoutCommand(string userId)
    {
        this.UserId = userId;
    }
}