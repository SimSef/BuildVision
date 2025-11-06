using Aspire.StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Costs.Infrastructure.Cache;

public static class CacheRegistration
{
    public static IHostApplicationBuilder AddCache(this IHostApplicationBuilder builder)
    {
        builder.AddRedisDistributedCache(connectionName: "costs-cache");

        builder.Services
            .AddFusionCache()
            .WithSerializer(new FusionCacheSystemTextJsonSerializer())
            .WithDefaultEntryOptions(opt => { opt.Duration = TimeSpan.FromMinutes(5); })
            .WithDistributedCache(sp => sp.GetRequiredService<IDistributedCache>(), null)
            .AsHybridCache();
        return builder;
    }
}
