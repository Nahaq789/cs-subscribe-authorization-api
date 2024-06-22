using Authorization.API.domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.API.infrastructure.entityConfiguration;

internal class UserAggregateConfiguration : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.ToTable("user_aggregate");
        builder.Property(x => x.AggregateId)
            .HasColumnName("user_aggregate_id");
    }
}