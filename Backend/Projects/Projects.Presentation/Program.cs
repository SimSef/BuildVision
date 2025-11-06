using Projects.Infrastructure;
using Projects.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfra();

var app = builder.Build();

await app.Services.ApplyProjectsDbMigrationsAsync();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
