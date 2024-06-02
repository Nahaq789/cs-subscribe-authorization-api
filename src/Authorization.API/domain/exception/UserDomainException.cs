namespace Authorization.API.domain.exception;

public class UserDomainException : Exception
{
    public UserDomainException() { }
    public UserDomainException(string message) : base(message) { }
    public UserDomainException(string message, Exception innerException) : base(message, innerException) { }
}