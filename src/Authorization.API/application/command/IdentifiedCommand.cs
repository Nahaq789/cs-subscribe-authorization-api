using MediatR;

namespace Authorization.API.application.command;

public class IdentifiedCommand<T, R> : IRequest<R>
    where T : IRequest<R>
{
    public T Command { get; }
    public Guid RequestId { get; }

    public IdentifiedCommand(T command, Guid requestId)
    {
        this.Command = command;
        this.RequestId = requestId;
    }
}