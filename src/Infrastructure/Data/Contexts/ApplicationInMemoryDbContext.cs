using DotnetEventSourcing.src.Core.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace DotnetEventSourcing.src.Infrastructure.Data.Contexts;

public class ApplicationInMemoryDbContext(DbContextOptions<ApplicationInMemoryDbContext> opt) : BaseDbContext(opt), IDb<ApplicationInMemoryDbContext>
{
    public const string DatabaseName = "DotnetEventSourcingInMemoryDb";

    public ApplicationInMemoryDbContext GetDbContext() => this;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationInMemoryDbContext).Assembly,
            type => type.Namespace?.Split(".").LastOrDefault() == GetType().Name &&
                    type.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                    )
        );
    }
}