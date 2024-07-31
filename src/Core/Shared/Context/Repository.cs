using System.Linq.Expressions;
using DotnetEventSourcing.src.Core.Shared.Extensions;
using DotnetEventSourcing.src.Core.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DotnetEventSourcing.src.Core.Shared.Context;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual IPagedList<TEntity> GetPagedList(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToPagedList(pageIndex, pageSize);
        }
        else
        {
            return query.ToPagedList(pageIndex, pageSize);
        }
    }
    public virtual Task<IPagedList<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }

        return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
    }

    public virtual IPagedList<TResult> GetPagedList<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true
    ) where TResult : class
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).Select(selector).ToPagedList(pageIndex, pageSize);
        }

        return query.Select(selector).ToPagedList(pageIndex, pageSize);
    }

    public virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    ) where TResult : class
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }

        return query.Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
    }

    public virtual TEntity? GetFirstOrDefault(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).FirstOrDefault();
        }

        return query.FirstOrDefault();
    }

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        return await GetFirstOrDefaultAsync(predicate, null, null, disableTracking, cancellationToken);
    }

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).FirstOrDefaultAsync(cancellationToken);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual TResult? GetFirstOrDefault<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).Select(selector).FirstOrDefault();
        }

        return query.Select(selector).FirstOrDefault();
    }

    public virtual async Task<TResult?> GetFirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).Select(selector).FirstOrDefaultAsync(cancellationToken);
        }

        return await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual IQueryable<TEntity> FromSql(string sql, params object[] parameters) => _dbSet.FromSqlRaw(sql, parameters);

    public virtual TEntity? Find(params object[] keyValues) => _dbSet.Find(keyValues);

    public virtual Task<TEntity?> FindAsync(params object[] keyValues) => _dbSet.FindAsync(keyValues).AsTask();

    public virtual Task<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken) => _dbSet.FindAsync(keyValues, cancellationToken).AsTask();

    public virtual int Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return _dbSet.Count();
        }

        return _dbSet.Count(predicate);
    }

    public virtual void Insert(TEntity entity) => _dbSet.Add(entity);

    public virtual void Insert(params TEntity[] entities) => _dbSet.AddRange(entities);

    public virtual void Insert(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

    public virtual Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default) => _dbSet.AddAsync(entity, cancellationToken).AsTask();

    public virtual Task InsertAsync(params TEntity[] entities) => _dbSet.AddRangeAsync(entities);

    public virtual Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) => _dbSet.AddRangeAsync(entities, cancellationToken);

    public virtual void Update(TEntity entity) => _dbSet.Update(entity);

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual void Update(params TEntity[] entities) => _dbSet.UpdateRange(entities);

    public virtual void Update(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    public virtual void Delete(TEntity entity) => _dbSet.Remove(entity);

    public virtual void Delete(object id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public virtual void Delete(params TEntity[] entities) => _dbSet.RemoveRange(entities);

    public virtual void Delete(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    public IPagedList<TEntity> Find(Expression<Func<TEntity, bool>>? expression = null)
    {
        return GetPagedList(expression);
    }

    public Task<IPagedList<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        return GetPagedListAsync(expression);
    }

    public Task UpdateAsync(params TEntity[] entities)
    {
        Update(entities);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        Update(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(object id)
    {
        Delete(id);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        Delete(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity[] entities)
    {
        Delete(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        Delete(entities);
        return Task.CompletedTask;
    }

    public ITotalPagedResult<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return new TotalPagedResult<TEntity>(query);
    }

    public virtual async Task<ITotalPagedResult<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return new TotalPagedResult<TEntity>(query);
    }

    public ITotalPagedResult<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return new TotalPagedResult<TEntity>(orderBy(query).AsQueryable());
        }

        return new TotalPagedResult<TEntity>(query.AsQueryable());
    }

    public virtual async Task<ITotalPagedResult<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        IQueryable<TEntity> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return new TotalPagedResult<TEntity>(orderBy(query).AsQueryable());
        }

        return new TotalPagedResult<TEntity>(query.AsQueryable());
    }

    public IQueryable<TEntity> GetQueryale() => _dbSet;

    public TEntity? GetLastOrDefault(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).LastOrDefault();
        }

        return query.LastOrDefault();
    }

    public TResult? GetLastOrDefault<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return orderBy(query).Select(selector).LastOrDefault();
        }

        return query.Select(selector).LastOrDefault();
    }

    public async Task<TResult?> GetLastOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).Select(selector).LastOrDefaultAsync(cancellationToken);
        }

        return await query.Select(selector).LastOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> GetLastOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = _dbSet;
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).LastOrDefaultAsync(cancellationToken);
        }

        return await query.LastOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity?> GetLastOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default
    ) => GetLastOrDefaultAsync(predicate, null, null, disableTracking, cancellationToken);
}