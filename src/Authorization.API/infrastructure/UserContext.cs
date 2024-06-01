using Authorization.API.domain;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.infrastructure;

public class UserContext : DbContext
{
    public DbSet<UserAuth> UserAuth { get; set; }

    public UserContext() { }
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}