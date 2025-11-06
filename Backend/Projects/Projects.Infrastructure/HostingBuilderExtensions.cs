using Microsoft.Extensions.Hosting;
using Projects.Infrastructure.Observability;

namespace Projects.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetry();
        return builder;
    }
}
