using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Projects.Infrastructure.Persistence;

public sealed class ProjectsDbContextFactory : IDesignTimeDbContextFactory<ProjectsDbContext>
{
    public ProjectsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectsDbContext>();
        const string cs = "Host=localhost;Database=dummy;Username=dummy;Password=dummy";
        optionsBuilder.UseNpgsql(cs);
        return new ProjectsDbContext(optionsBuilder.Options);
    }
}

