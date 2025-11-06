using Microsoft.Extensions.Hosting;
using Projects.Infrastructure.Observability;
using Projects.Infrastructure.Persistence;
using Projects.Infrastructure.Messaging;
using Projects.Infrastructure.Cache;

namespace Projects.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetry();
        builder.AddPersistence();
        builder.AddMessaging();
        builder.AddCache();
        return builder;
    }
}
