using Microsoft.Extensions.Hosting;
using Costs.Infrastructure.Observability;
using Costs.Infrastructure.Persistence;
using Costs.Infrastructure.Messaging;
using Costs.Infrastructure.Cache;

namespace Costs.Infrastructure;

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
