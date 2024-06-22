using System.ComponentModel.DataAnnotations;

namespace Authorization.API.domain;

public class UserAggregate
{
    [Key]
    public Guid AggregateId { get; private set; }

    public UserAggregate(Guid aggregateId)
    {
        this.AggregateId = aggregateId;
    }
}