using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Costs.Infrastructure.Persistence;

public sealed class CostsDbContextFactory : IDesignTimeDbContextFactory<CostsDbContext>
{
    public CostsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CostsDbContext>();
        const string cs = "Host=localhost;Database=dummy;Username=dummy;Password=dummy";
        optionsBuilder.UseNpgsql(cs);
        return new CostsDbContext(optionsBuilder.Options);
    }
}

