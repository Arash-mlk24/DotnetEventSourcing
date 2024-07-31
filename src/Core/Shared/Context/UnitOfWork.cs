using System.Data;
using System.Text.RegularExpressions;
using System.Transactions;
using DotnetEventSourcing.src.Core.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Z.EntityFramework.Plus;

namespace DotnetEventSourcing.src.Core.Shared.Context;

public abstract class UnitOfWorkBase<TContext> where TContext : IDb<TContext>
{
    protected readonly IDb<TContext> _dbContextGetter;
    protected TContext _context { get; private set; }
    protected bool disposed = false;
    protected Dictionary<Type, object> repositories = null!;

    public UnitOfWorkBase(IDb<TContext> dbContextGetter)
    {
        _dbContextGetter = dbContextGetter ?? throw new ArgumentNullException(nameof(_context));
        _context = dbContextGetter.GetDbContext();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                repositories?.Clear();

                _context.Dispose();
            }
        }

        disposed = true;
    }
}

public class UnitOfWork<TContext> : UnitOfWorkBase<TContext>, IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork
where TContext : DbContext, IDb<TContext>
{
    public UnitOfWork(TContext context) : base(context)
    {
        context.GetDbContext();
    }

    public TContext DbContext => _context;

    public void ChangeDatabase(string database)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State.HasFlag(ConnectionState.Open))
        {
            connection.ChangeDatabase(database);
        }
        else
        {
            var connectionString = Regex.Replace(connection.ConnectionString.Replace(" ", ""), @"(?<=[Dd]atabase=)\w+(?=;)", database, RegexOptions.Singleline);
            connection.ConnectionString = connectionString;
        }

        _context.Model.GetEntityTypes();
    }

    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : BaseEntity
    {
        repositories ??= [];

        if (hasCustomRepository)
        {
            var customRepo = _context.GetService<IRepository<TEntity>>();
            if (customRepo != null)
            {
                return customRepo;
            }
        }

        var type = typeof(TEntity);
        if (!repositories.TryGetValue(type, out object? value))
        {
            value = new Repository<TEntity>(_context);
            repositories[type] = value;
        }

        return (IRepository<TEntity>)value;
    }

    public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlRaw(sql, parameters);

    public IQueryable<TEntity> FromSql<TEntity>(
        string sql,
        params object[] parameters
    ) where TEntity : BaseEntity => _context.Set<TEntity>().FromSqlRaw(sql, parameters);

    public int SaveChanges()
    {
        var res = _context.SaveChanges();

        var attached = _context.ChangeTracker.Entries().Where(x => x.Entity is BaseEntity).ToList();

        foreach (var entry in attached)
        {
            _context.Entry(entry.Entity).State = EntityState.Detached;
        }

        return res;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        var count = 0;

        foreach (var unitOfWork in unitOfWorks)
        {
            count += await unitOfWork.SaveChangesAsync();
        }

        count += await SaveChangesAsync();

        ts.Complete();

        return count;
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
    {
        _context.ChangeTracker.TrackGraph(rootEntity, callback);
    }
}