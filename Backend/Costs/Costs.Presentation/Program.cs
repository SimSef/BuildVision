using Costs.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfra();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
