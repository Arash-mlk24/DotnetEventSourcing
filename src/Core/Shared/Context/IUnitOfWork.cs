using DotnetEventSourcing.src.Core.Shared.Models;

namespace DotnetEventSourcing.src.Core.Shared.Context;

public interface IBaseUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : BaseEntity;
}

public interface IUnitOfWork : IBaseUnitOfWork
{
    public void ChangeDatabase(string database);

    public int SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int ExecuteSqlCommand(string sql, params object[] parameters);

    IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity;
}