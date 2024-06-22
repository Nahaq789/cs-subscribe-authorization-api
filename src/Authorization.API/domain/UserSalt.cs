using System.ComponentModel.DataAnnotations;

namespace Authorization.API.domain;

public class UserSalt
{
    [Key]
    public long SaltId { get; set; }
    public string Salt { get; private set; }
    public Guid AggregateId { get; private set; }

    public UserSalt(Guid aggregateId, string salt)
    {
        this.AggregateId = aggregateId;
        this.Salt = salt;
    }
}