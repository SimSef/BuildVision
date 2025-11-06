using Microsoft.Extensions.Hosting;
using Gateway.Infrastructure.Observability;

namespace Gateway.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        // Observability (OpenTelemetry)
        builder.AddOpenTelemetry();
        // Infrastructure wiring will be added here (DB, messaging, etc.).
        return builder;
    }
}
