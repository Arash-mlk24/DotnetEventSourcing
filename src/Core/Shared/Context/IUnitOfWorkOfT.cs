namespace DotnetEventSourcing.src.Core.Shared.Context;

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IDb<TContext>
{
    TContext DbContext { get; }

    Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks);
}