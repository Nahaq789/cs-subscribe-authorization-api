namespace Authorization.infrastructure.idempotency;

public interface IRequestManager
{
    Task<bool> ExistAsync(Guid id);
    Task CreateRequestForCommandAsync<T>(Guid id);
}