using Authorization.API.domain.exception;
using Authorization.API.infrastructure;

namespace Authorization.infrastructure.idempotency;

public class RequestManager : IRequestManager
{
    private readonly UserContext _userContext;

    public RequestManager(UserContext userContext)
    {
        this._userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await _userContext.FindAsync<ClientRequest>(id);

        return request != null;
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exists = await ExistAsync(id);

        var request = exists ? throw new UserDomainException($"Request ID:{id} はすでに存在し、重複しています。") : new ClientRequest()
        {
            Id = id,
            Name = typeof(T).Name,
            Time = DateTime.UtcNow
        };

        _userContext.Add(request);
        await _userContext.SaveChangesAsync();
    }
}