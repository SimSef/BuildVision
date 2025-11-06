using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Costs.Infrastructure.Persistence;

public static class CostsMigrationExtensions
{
    public static async Task ApplyCostsDbMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CostsDbContext>>();
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<CostsDbContext>();
            await db.Database.MigrateAsync();
            logger.LogInformation("Costs database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error applying Costs database migrations before app run.");
            throw;
        }
    }
}
