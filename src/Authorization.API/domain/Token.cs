namespace Authorization.API.domain;

public class Token
{
    public string JwtToken { get; private set; }
    public Token(string jwtToken)
    {
        this.JwtToken = jwtToken;
    }
}