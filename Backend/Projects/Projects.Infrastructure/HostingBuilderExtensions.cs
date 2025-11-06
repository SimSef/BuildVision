using Microsoft.Extensions.Hosting;

namespace Projects.Infrastructure;

public static class HostingBuilderExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        // Infrastructure wiring will be added here (DB, messaging, OTel, etc.).
        return builder;
    }
}

