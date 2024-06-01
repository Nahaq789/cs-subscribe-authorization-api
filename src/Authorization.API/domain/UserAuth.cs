namespace Authorization.API.domain;

public class UserAuth
{
    public string Email { get; private set; }
    public string Password { get; private set; }

    public UserAuth(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
}