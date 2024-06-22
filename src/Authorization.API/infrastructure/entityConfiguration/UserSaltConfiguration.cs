using Authorization.API.domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.API.infrastructure.entityConfiguration;

internal class UserSaltConfiguration : IEntityTypeConfiguration<UserSalt>
{
    public void Configure(EntityTypeBuilder<UserSalt> builder)
    {
        builder.ToTable("user_salt");
        builder.Property(x => x.Salt).HasColumnName("salt");
        builder.Property(x => x.AggregateId).HasColumnName("user_aggregate_id");
    }
}