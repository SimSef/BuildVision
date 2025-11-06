using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure;

namespace Gateway.Infrastructure.Persistence;

public class GatewayDbContext : DbContext
{
    public GatewayDbContext(DbContextOptions<GatewayDbContext> options) : base(options) { }

    public DbSet<OutboxMessage> Outbox => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var o = modelBuilder.Entity<OutboxMessage>();
        o.ToTable("outbox");

        o.HasKey(x => x.Id);
        o.Property(x => x.Id).HasColumnName("id");
        o.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        o.Property(x => x.Status).HasColumnName("status").HasDefaultValue("PENDING");
        o.Property(x => x.SentAt).HasColumnName("sent_at").HasColumnType("timestamptz");
        o.Property(x => x.Retries).HasColumnName("retries").HasDefaultValue(0);
        o.Property(x => x.ErrorType).HasColumnName("error_type");

        o.Property(x => x.AggregateType).HasColumnName("aggregate_type");
        o.Property(x => x.AggregateId).HasColumnName("aggregate_id");

        o.Property(x => x.CeId).HasColumnName("ce_id");
        o.Property(x => x.CeSource).HasColumnName("ce_source");
        o.Property(x => x.CeType).HasColumnName("ce_type");
        o.Property(x => x.CeTime).HasColumnName("ce_time").HasColumnType("timestamptz");
        o.Property(x => x.CeSubject).HasColumnName("ce_subject");
        o.Property(x => x.CeDataschema).HasColumnName("ce_dataschema");
        o.Property(x => x.CeDatacontenttype).HasColumnName("ce_datacontenttype");

        o.Property(x => x.CeTraceparent).HasColumnName("ce_traceparent");
        o.Property(x => x.CeTracestate).HasColumnName("ce_tracestate");
        o.Property(x => x.CeConversationid).HasColumnName("ce_conversationid");
        o.Property(x => x.CeTenant).HasColumnName("ce_tenant");

        o.Property(x => x.Payload).HasColumnName("payload").HasColumnType("jsonb");

        o.Property(x => x.DestinationName).HasColumnName("destination_name");
        o.Property(x => x.SubscriptionName).HasColumnName("subscription_name");

        o.Property(x => x.OpName).HasColumnName("op_name").HasDefaultValue("send");
        o.Property(x => x.OpType).HasColumnName("op_type").HasDefaultValue("send");
        o.Property(x => x.ServerAddress).HasColumnName("server_address");
        o.Property(x => x.ServerPort).HasColumnName("server_port");

        o.HasIndex(x => new { x.Status, x.CreatedAt }).HasDatabaseName("idx_outbox_status_created_at");
        o.HasIndex(x => x.DestinationName).HasDatabaseName("idx_outbox_destination");
        o.HasIndex(x => x.CeType).HasDatabaseName("idx_outbox_cetype");
        o.HasIndex(x => x.CeConversationid).HasDatabaseName("idx_outbox_conversation");
    }
}
