using Microsoft.Extensions.Hosting;
using Gateway.Infrastructure.Observability;
using Gateway.Infrastructure.Persistence;
using Gateway.Infrastructure.Messaging;
using Gateway.Infrastructure.Cache;

namespace Gateway.Infrastructure;

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
