using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Costs.Infrastructure.Persistence;

public static class PersistenceRegistration
{
    public static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<CostsDbContext>(
            connectionName: "costsdb",
            configureDbContextOptions: options =>
            {
                options.ConfigureWarnings(w =>
                    w.Log(RelationalEventId.PendingModelChangesWarning));
            });

        return builder;
    }
}

