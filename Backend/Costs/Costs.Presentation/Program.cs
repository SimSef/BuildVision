using Costs.Infrastructure;
using Costs.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfra();

var app = builder.Build();

await app.Services.ApplyCostsDbMigrationsAsync();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
