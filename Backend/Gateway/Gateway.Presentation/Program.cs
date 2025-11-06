using Gateway.Infrastructure;
using Gateway.Infrastructure.Persistence;

using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);
builder.AddInfra();

builder.Services.AddOpenApi();

// AuthN/Z: Keycloak (JWT Bearer) via Aspire client integration
builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer(
        serviceName: "keycloak",
        realm: "BuildVision",
        options =>
        {
            if (builder.Environment.IsDevelopment())
            {
                options.RequireHttpsMetadata = false;
            }
        });

builder.Services.AddAuthorizationBuilder();

// Dev CORS to enable SPA calls during local dev
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(o =>
        o.AddPolicy("dev", p => p
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));
}

var app = builder.Build();

await app.Services.ApplyGatewayDbMigrationsAsync();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("dev");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

// Simple secured ping to validate tokens from SPA
app.MapGet("/api/ping", () => Results.Ok(new { ok = true }))
    .RequireAuthorization();

app.Run();
