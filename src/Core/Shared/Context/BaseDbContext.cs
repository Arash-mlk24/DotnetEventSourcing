using Microsoft.EntityFrameworkCore;

namespace DotnetEventSourcing.src.Core.Shared.Context;

public abstract class BaseDbContext : DbContext, IDb<BaseDbContext>
{
    public BaseDbContext()
    {
    }

    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    public BaseDbContext GetDbContext() => this;

    public TDb GetDbContext<TDb>() where TDb : class
    {
        return (GetDbContext() as TDb)!;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}