using Gateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Register infrastructure
builder.AddInfra();

// Keep OpenAPI only in Gateway
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Minimal placeholder endpoint
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
