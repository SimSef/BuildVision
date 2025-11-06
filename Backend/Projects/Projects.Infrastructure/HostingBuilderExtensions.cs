using Microsoft.Extensions.Hosting;
using Projects.Infrastructure.Observability;
using Projects.Infrastructure.Persistence;
using Projects.Infrastructure.Messaging;

namespace Projects.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetry();
        builder.AddPersistence();
        builder.AddMessaging();
        return builder;
    }
}
