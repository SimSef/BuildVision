using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Costs.Infrastructure.Observability;

public static class ObservabilityRegistration
{
    public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder)
    {
        const string serviceName = "BuildVision.Costs";
        const string serviceNamespace = "BuildVision";
        var serviceVersion = GetInformationalVersion() ?? "0.0.0";

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            serviceName: serviceName,
            serviceNamespace: serviceNamespace,
            serviceVersion: serviceVersion,
            serviceInstanceId: Environment.MachineName);

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(rb => rb.AddService(
                serviceName: serviceName,
                serviceNamespace: serviceNamespace,
                serviceVersion: serviceVersion,
                serviceInstanceId: Environment.MachineName))
            .WithTracing(tracer =>
            {
                tracer
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddOtlpExporter();
            })
            .WithMetrics(meter =>
            {
                meter
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter();
            });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;
            logging.SetResourceBuilder(resourceBuilder);
            logging.AddOtlpExporter();
        });

        return builder;
    }

    private static string? GetInformationalVersion()
    {
        var asm = Assembly.GetExecutingAssembly();
        var attr = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return attr?.InformationalVersion;
    }
}

