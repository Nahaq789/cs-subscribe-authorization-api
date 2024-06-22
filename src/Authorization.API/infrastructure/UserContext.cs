using Authorization.API.domain;
using Authorization.API.domain.dto;
using Authorization.API.infrastructure.entityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.infrastructure;

public class UserContext : DbContext
{
    // public DbSet<UserAuth> UserAuth { get; set; }
    public DbSet<UserAggregate> UserAggregate { get; set; }
    public DbSet<UserEntity> User { get; set; }
    public DbSet<UserSalt> UserSalt { get; set; }

    public UserContext() { }
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserAggregateConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserSaltConfiguration());
    }
}