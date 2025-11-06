using Microsoft.Extensions.Hosting;
using Costs.Infrastructure.Observability;
using Costs.Infrastructure.Persistence;

namespace Costs.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetry();
        builder.AddPersistence();
        return builder;
    }
}
