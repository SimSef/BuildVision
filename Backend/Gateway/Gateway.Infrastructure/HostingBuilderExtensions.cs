using Microsoft.Extensions.Hosting;
using Gateway.Infrastructure.Observability;
using Gateway.Infrastructure.Persistence;
using Gateway.Infrastructure.Messaging;

namespace Gateway.Infrastructure;

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
