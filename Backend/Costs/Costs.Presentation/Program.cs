using Costs.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Register infrastructure
builder.AddInfra();

var app = builder.Build();

// Minimal placeholder endpoint (no OpenAPI here)
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
