namespace Authorization.API.domain;

public class UserAuth
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Salt { get; private set; }

    public UserAuth(Guid userId, string email, string password, string salt)
    {
        this.UserId = userId;
        this.Email = email;
        this.Password = password;
        this.Salt = salt;
    }
}