using Microsoft.Extensions.Hosting;
using Gateway.Infrastructure.Observability;

namespace Gateway.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetry();
        return builder;
    }
}
