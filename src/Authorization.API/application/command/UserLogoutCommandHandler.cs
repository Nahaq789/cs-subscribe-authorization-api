using Authorization.API.application.service;
using MediatR;

namespace Authorization.API.application.command;

public class UserLogoutCommandHandler : IRequestHandler<UserLogoutCommand, string>
{
    private readonly IRefreshTokenService _refreshTokenService;

    public UserLogoutCommandHandler(IRefreshTokenService refreshTokenService)
    {
        this._refreshTokenService = refreshTokenService;
    }

    public async Task<string> Handle(UserLogoutCommand command, CancellationToken cancellationToken)
    {
        await _refreshTokenService.RemoveRefreshToken(command.UserId, cancellationToken);
        return "logout";
    }
}