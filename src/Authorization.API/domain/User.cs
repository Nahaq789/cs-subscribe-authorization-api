using System.ComponentModel.DataAnnotations;

namespace Authorization.API.domain;

public class UserEntity
{
    [Key]
    public long UserId { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public Guid AggregateId { get; private set; }

    public UserEntity(string email, string password, Guid aggregateId)
    {
        this.Email = email;
        this.Password = password;
        this.AggregateId = aggregateId;
    }
}