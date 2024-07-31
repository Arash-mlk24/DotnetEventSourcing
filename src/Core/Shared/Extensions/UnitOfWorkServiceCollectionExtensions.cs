using DotnetEventSourcing.src.Core.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetEventSourcing.src.Core.Shared.Extensions;

public static class UnitOfWorkServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext, IDb<TContext>
    {
        services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

        return services;
    }

    public static IServiceCollection AddUnitOfWork<TContext1, TContext2>(this IServiceCollection services)
        where TContext1 : DbContext, IDb<TContext1>
        where TContext2 : DbContext, IDb<TContext2>
    {
        services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
        services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();

        return services;
    }

    public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3>(this IServiceCollection services)
        where TContext1 : DbContext, IDb<TContext1>
        where TContext2 : DbContext, IDb<TContext2>
        where TContext3 : DbContext, IDb<TContext3>
    {
        services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
        services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
        services.AddScoped<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();

        return services;
    }

    public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3, TContext4>(this IServiceCollection services)
        where TContext1 : DbContext, IDb<TContext1>
        where TContext2 : DbContext, IDb<TContext2>
        where TContext3 : DbContext, IDb<TContext3>
        where TContext4 : DbContext, IDb<TContext4>
    {
        services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
        services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
        services.AddScoped<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();
        services.AddScoped<IUnitOfWork<TContext4>, UnitOfWork<TContext4>>();

        return services;
    }
}
