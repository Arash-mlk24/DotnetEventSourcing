using System.Reflection;
using DotnetEventSourcing.src.Core.Shared.Extensions;
using DotnetEventSourcing.src.Core.Utilities;
using DotnetEventSourcing.src.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DotnetEventSourcing.src.Web.Utilities.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationCors(this IServiceCollection services, string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                name: policyName,
                builder => builder.SetIsOriginAllowed(x => true).AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
            );
        });
    }

    public static void AddApplicationApis(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }

    public static void AddApplicationContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(configuration);
        services.AddDbContext<ApplicationInMemoryDbContext>(options => options.UseInMemoryDatabase(ApplicationInMemoryDbContext.DatabaseName));
        services.AddUnitOfWork<ApplicationDbContext>();
        services.AddUnitOfWork<ApplicationInMemoryDbContext>();
    }

    public static void AddApplicationSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
    }

    public static void AddApplicationMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }

    public static void RegisterMapster(this IServiceCollection _)
    {
        new MapsterConfiguration().Register(Mapster.TypeAdapterConfig.GlobalSettings);
    }
}