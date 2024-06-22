using Authorization.API.domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.API.infrastructure.entityConfiguration;

internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("user");

        builder.Property(x => x.AggregateId)
            .HasColumnName("user_aggregate_id");
        builder.Property(x => x.Email)
            .HasColumnName("email");
        builder.Property(x => x.Password)
            .HasColumnName("password");
    }
}
