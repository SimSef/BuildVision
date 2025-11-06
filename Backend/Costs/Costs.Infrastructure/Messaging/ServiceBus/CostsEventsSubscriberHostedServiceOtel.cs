using System.Diagnostics;
using System.Collections.Generic;
using OpenTelemetry.Context.Propagation;

namespace Costs.Infrastructure.Messaging.ServiceBus;

internal static class CostsEventsSubscriberHostedServiceOtel
{
    private static readonly ActivitySource Source = new("BuildVision.Costs.Infrastructure");

    public static Activity? StartConsume(string queueName, string messageId, string? subject, IReadOnlyDictionary<string, object?> appProps)
    {
        var links = BuildLinks(appProps);
        var activity = Source.StartActivity(
            "servicebus.message.receive",
            ActivityKind.Consumer,
            parentContext: default,
            tags: null,
            links: links);

        activity?.SetTag("messaging.system", "azureservicebus");
        activity?.SetTag("messaging.destination.kind", "queue");
        activity?.SetTag("messaging.destination.name", queueName);
        activity?.SetTag("messaging.operation", "process");
        activity?.SetTag("messaging.message_id", messageId);
        if (!string.IsNullOrWhiteSpace(subject)) activity?.SetTag("event.subject", subject);
        return activity;
    }

    private static ActivityLink[] BuildLinks(IReadOnlyDictionary<string, object?> appProps)
    {
        string? traceparent = null;
        string? tracestate = null;

        if (appProps.TryGetValue("traceparent", out var tpObj) && tpObj is string tpStr)
            traceparent = tpStr;

        if (appProps.TryGetValue("tracestate", out var tsObj) && tsObj is string tsStr)
            tracestate = tsStr;

        var carrier = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            ["traceparent"] = traceparent,
            ["tracestate"] = tracestate
        };

        var parent = Propagators.DefaultTextMapPropagator.Extract(
            default,
            carrier,
            static (c, k) => c.TryGetValue(k, out var v) && v != null ? new[] { v } : Array.Empty<string>());

        return parent.ActivityContext != default
            ? [new ActivityLink(parent.ActivityContext)]
            : [];
    }

    public static Task OnErrorAsync(string queueName, Exception ex)
    {
        using var activity = StartConsume(
            queueName: queueName,
            messageId: string.Empty,
            subject: null,
            appProps: new Dictionary<string, object?>());
        activity?.SetTag("exception.type", ex.GetType().FullName);
        activity?.SetTag("exception.message", ex.Message);
        return Task.CompletedTask;
    }
}

