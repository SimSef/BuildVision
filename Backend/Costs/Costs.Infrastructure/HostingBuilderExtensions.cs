using Microsoft.Extensions.Hosting;
using Costs.Infrastructure.Observability;

namespace Costs.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetry();
        return builder;
    }
}
