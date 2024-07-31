using DotnetEventSourcing.src.Core.Shared.Context;
using DotnetEventSourcing.src.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DotnetEventSourcing.src.Web.Utilities.Extensions;

public static class ApplicationExtensions
{
    public static void UseApplicationSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void ApplyDatabaseMigrate(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        var unitOfWork = serviceScope.ServiceProvider.GetService<IUnitOfWork<ApplicationDbContext>>();
        unitOfWork?.DbContext.Database.Migrate();
    }
}