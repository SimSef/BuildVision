using System.Diagnostics;

namespace Projects.Infrastructure.Messaging.ServiceBus;

internal static class OutboxPublisherHostedServiceOtel
{
    private static readonly ActivitySource Source = new("BuildVision.Projects.Infrastructure");
    public static Activity? StartPublish(string destination, string messageId, string conversationId, string? traceparent, string? tracestate)
    {
        var links = BuildLinks(traceparent, tracestate);
        var activity = Source.StartActivity(
            "servicebus.outbox.publish",
            ActivityKind.Producer,
            parentContext: default,
            tags: null,
            links: links);

        activity?.SetTag("messaging.system", "azureservicebus");
        activity?.SetTag("messaging.destination.kind", "topic");
        activity?.SetTag("messaging.destination.name", destination);
        activity?.SetTag("messaging.operation", "publish");
        activity?.SetTag("messaging.message_id", messageId);
        activity?.SetTag("messaging.message.conversation_id", conversationId);
        return activity;
    }

    private static ActivityLink[] BuildLinks(string? traceparent, string? tracestate)
    {
        var carrier = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            ["traceparent"] = traceparent,
            ["tracestate"] = tracestate
        };

        var parent = OpenTelemetry.Context.Propagation.Propagators.DefaultTextMapPropagator.Extract(
            default,
            carrier,
            static (c, k) => c.TryGetValue(k, out var v) && v != null ? new[] { v } : Array.Empty<string>());

        return parent.ActivityContext != default
            ? [new ActivityLink(parent.ActivityContext)]
            : [];
    }
}
