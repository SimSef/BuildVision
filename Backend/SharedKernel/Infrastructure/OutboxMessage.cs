using System.Text.Json;

namespace SharedKernel.Infrastructure;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Status { get; set; } = "PENDING";
    public DateTimeOffset? SentAt { get; set; }
    public int Retries { get; set; }
    public string? ErrorType { get; set; }

    public string AggregateType { get; set; } = null!;
    public string AggregateId { get; set; } = null!;

    public string CeId { get; set; } = null!;
    public string CeSource { get; set; } = null!;
    public string CeType { get; set; } = null!;
    public DateTimeOffset CeTime { get; set; }
    public string? CeSubject { get; set; }
    public string? CeDataschema { get; set; }
    public string CeDatacontenttype { get; set; } = null!;

    public string CeTraceparent { get; set; } = null!;
    public string? CeTracestate { get; set; }
    public string? CeConversationid { get; set; }
    public string? CeTenant { get; set; }

    public JsonDocument Payload { get; set; } = null!;

    public string DestinationName { get; set; } = null!;
    public string? SubscriptionName { get; set; }

    public string OpName { get; set; } = "send";
    public string OpType { get; set; } = "send";
    public string? ServerAddress { get; set; }
    public int? ServerPort { get; set; }
}

