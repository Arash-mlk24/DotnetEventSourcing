namespace DotnetEventSourcing.src.Core.Shared.Context;

public interface IDb : IDisposable
{
    TDb GetDbContext<TDb>() where TDb : class;
}

public interface IDb<out TDb> : IDb
{
    TDb GetDbContext();
}