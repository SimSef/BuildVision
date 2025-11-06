using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Gateway.Infrastructure.Persistence;

public static class PersistenceRegistration
{
    public static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<GatewayDbContext>(
            connectionName: "gatewaydb",
            configureDbContextOptions: options =>
            {
                options.ConfigureWarnings(w =>
                    w.Log(RelationalEventId.PendingModelChangesWarning));
            });

        return builder;
    }
}

