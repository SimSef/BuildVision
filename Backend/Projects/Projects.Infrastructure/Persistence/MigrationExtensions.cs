using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Projects.Infrastructure.Persistence;

public static class ProjectsMigrationExtensions
{
    public static async Task ApplyProjectsDbMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProjectsDbContext>>();
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<ProjectsDbContext>();
            await db.Database.MigrateAsync();
            logger.LogInformation("Projects database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error applying Projects database migrations before app run.");
            throw;
        }
    }
}
