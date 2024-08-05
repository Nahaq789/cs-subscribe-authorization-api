using Authorization.API.application.command;
using Authorization.API.application.model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace User.Authentication.presentation.controller;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Loginエンドポイント
    /// ユーザーログインコマンドハンドラーを呼び出します。
    /// </summary>
    ///<param name="command">ユーザーログインコマンド</param>
    ///<param name="requestId">リクエストID</param>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AuthResult>, BadRequest<string>, ProblemHttpResult>> Login(
        [FromBody] UserLoginCommand command, [FromHeader(Name = "x-requestId")] Guid requestId
    )
    {
        if (requestId == Guid.Empty)
        {
            return TypedResults.BadRequest("Request ID is empty");
        }
        var requestCommand = new IdentifiedCommand<UserLoginCommand, AuthResult>(command, requestId);

        _logger.LogInformation("Sending command: {CommandName})", requestCommand.GetType());

        var commandResult = await _mediator.Send(requestCommand.Command);
        if (commandResult.Success == false)
        {
            return TypedResults.Problem(detail: commandResult.ErrorMessage, statusCode: 401);
        }
        return TypedResults.Ok(commandResult);
    }

    /// <summary>
    /// Logoutエンドポイント
    /// ユーザーログアウトコマンドハンドラーを呼び出します。
    /// </summary>
    ///<param name="command">ユーザーログアウトコマンド</param>
    ///<param name="requestId">リクエストID</param>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> Logout(
        [FromBody] UserLogoutCommand command, [FromHeader(Name = "x-requestId")] Guid requestId
    )
    {
        if (requestId == Guid.Empty)
        {
            return TypedResults.BadRequest("RequestID is Empty");
        }
        var requestCommand = new IdentifiedCommand<UserLogoutCommand, string>(command, requestId);

        _logger.LogInformation("Sending command: {CommandName})", requestCommand.GetType());

        await _mediator.Send(requestCommand);
        return TypedResults.Ok();
    }
}