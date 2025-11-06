using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gateway.Infrastructure.Persistence;

public static class GatewayMigrationExtensions
{
    public static async Task ApplyGatewayDbMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GatewayDbContext>>();
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<GatewayDbContext>();
            await db.Database.MigrateAsync();
            logger.LogInformation("Gateway database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error applying Gateway database migrations before app run.");
            throw;
        }
    }
}

