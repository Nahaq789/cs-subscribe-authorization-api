using Authorization.infrastructure.idempotency;
using MediatR;

namespace Authorization.API.application.command;

/// <summary>
/// クライアントが送信したリクエストIDを使用して重複確認を行います。
/// 重複していなければコマンドハンドラーを呼び出します。
/// </summary>
/// <typeparam name="T">コマンド</typeparam>
/// <typeparam name="R">コマンドハンドラーのレスポンス</typeparam>
public abstract class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
    where T : IRequest<R>
{
    private readonly IMediator _mediator;
    private readonly IRequestManager _requestManager;
    private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

    public IdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<T, R>> logger
    )
    {
        this._mediator = mediator;
        this._requestManager = requestManager;
        this._logger = logger;
    }

    /// <summary>
    /// リクエストが重複していた場合、返却する値を生成します。
    /// </summary>
    protected abstract R CreateResultForDuplicateRequest();

    /// <summary>
    /// 同じリクエストIDが重複していないか確認し、重複していなければコマンドハンドラーを呼び出します。
    /// </summary>
    /// <typeparam name="command">IdentifiedCommandは元のコマンドとリクエストIDの両方を含みます。</typeparam>
    public async Task<R> Handle(IdentifiedCommand<T, R> command, CancellationToken cancellationToken)
    {
        var already = await _requestManager.ExistAsync(command.RequestId);
        if (already)
        {
            return CreateResultForDuplicateRequest();
        }
        else
        {
            try
            {
                var message = command.Command;
                var messageType = command.GetType();
                var idProperty = string.Empty;
                var commandId = string.Empty;

                _logger.LogInformation(
                    "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    message,
                    idProperty,
                    commandId,
                    command);

                var result = await _mediator.Send(message, cancellationToken);

                _logger.LogInformation(
                    "Command result: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    message,
                    idProperty,
                    commandId,
                    command);

                return result;
            }
            catch
            {
#pragma warning disable CS8603 // Possible null reference return.

                return default;
#pragma warning restore CS8603 // Possible null reference return.

            }
        }
    }
}