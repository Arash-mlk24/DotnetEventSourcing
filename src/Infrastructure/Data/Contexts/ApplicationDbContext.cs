using DotnetEventSourcing.src.Core.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace DotnetEventSourcing.src.Infrastructure.Data.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : BaseDbContext(opt), IDb<ApplicationDbContext>
{
    public ApplicationDbContext GetDbContext() => this;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly,
            type => type.Namespace?.Split(".").LastOrDefault() == GetType().Name &&
                    type.GetInterfaces().Any(i => 
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                    )
        );
    }
}