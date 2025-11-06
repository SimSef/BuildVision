using Microsoft.Extensions.Hosting;
using Projects.Infrastructure.Observability;
using Projects.Infrastructure.Persistence;

namespace Projects.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetry();
        builder.AddPersistence();
        return builder;
    }
}
