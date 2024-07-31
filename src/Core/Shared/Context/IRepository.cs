using System.Linq.Expressions;
using DotnetEventSourcing.src.Core.Shared.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace DotnetEventSourcing.src.Core.Shared.Context;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    public IPagedList<TEntity> GetPagedList(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true
    );

    public Task<IPagedList<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    );

    public IPagedList<TResult> GetPagedList<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true
    ) where TResult : class;

    public Task<IPagedList<TResult>> GetPagedListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    ) where TResult : class;

    public TEntity? GetFirstOrDefault(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    );

    public TResult? GetFirstOrDefault<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    );

    public Task<TResult?> GetFirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    );

    public Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    );

    public Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    );

    public IQueryable<TEntity> FromSql(string sql, params object[] parameters);

    public TEntity? Find(params object[] keyValues);

    public IPagedList<TEntity> Find(Expression<Func<TEntity, bool>>? expression = null);

    public Task<IPagedList<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? expression = null);

    public Task<TEntity?> FindAsync(params object[] keyValues);

    public Task<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken);

    public ITotalPagedResult<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null);

    public Task<ITotalPagedResult<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default
    );

    public ITotalPagedResult<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
    );

    public Task<ITotalPagedResult<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default
    );

    public int Count(Expression<Func<TEntity, bool>>? predicate = null);

    public void Insert(TEntity entity);

    public void Insert(params TEntity[] entities);

    public void Insert(IEnumerable<TEntity> entities);

    public Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task InsertAsync(params TEntity[] entities);

    public Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public void Update(TEntity entity);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    public void Update(params TEntity[] entities);

    public Task UpdateAsync(params TEntity[] entities);

    public void Update(IEnumerable<TEntity> entities);

    public Task UpdateAsync(IEnumerable<TEntity> entities);

    public void Delete(object id);

    public Task DeleteAsync(object id);

    public void Delete(TEntity entity);

    public Task DeleteAsync(TEntity entity);

    public void Delete(params TEntity[] entities);

    public Task DeleteAsync(TEntity[] entities);

    public void Delete(IEnumerable<TEntity> entities);

    public Task DeleteAsync(IEnumerable<TEntity> entities);

    public IQueryable<TEntity> GetQueryale();

    public TEntity? GetLastOrDefault(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    );

    public TResult? GetLastOrDefault<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    );

    public Task<TResult?> GetLastOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    );

    public Task<TEntity?> GetLastOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    );

    public Task<TEntity?> GetLastOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    );
}