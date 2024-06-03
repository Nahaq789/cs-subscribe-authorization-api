namespace Authorization.API.application.service;

public interface ICryptoPasswordService
{
    string HashPassword(string password, string salt);
    string CreateSalt();
}