using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gateway.Infrastructure.Persistence;

public sealed class GatewayDbContextFactory : IDesignTimeDbContextFactory<GatewayDbContext>
{
    public GatewayDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GatewayDbContext>();
        const string cs = "Host=localhost;Database=dummy;Username=dummy;Password=dummy";
        optionsBuilder.UseNpgsql(cs);
        return new GatewayDbContext(optionsBuilder.Options);
    }
}

