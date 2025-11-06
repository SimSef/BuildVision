using Gateway.Infrastructure;
using Gateway.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfra();

builder.Services.AddOpenApi();

var app = builder.Build();

await app.Services.ApplyGatewayDbMigrationsAsync();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
