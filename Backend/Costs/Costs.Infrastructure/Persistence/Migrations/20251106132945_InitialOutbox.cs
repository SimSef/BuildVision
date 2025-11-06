using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costs.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "outbox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    sent_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    retries = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    error_type = table.Column<string>(type: "text", nullable: true),
                    aggregate_type = table.Column<string>(type: "text", nullable: false),
                    aggregate_id = table.Column<string>(type: "text", nullable: false),
                    ce_id = table.Column<string>(type: "text", nullable: false),
                    ce_source = table.Column<string>(type: "text", nullable: false),
                    ce_type = table.Column<string>(type: "text", nullable: false),
                    ce_time = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ce_subject = table.Column<string>(type: "text", nullable: true),
                    ce_dataschema = table.Column<string>(type: "text", nullable: true),
                    ce_datacontenttype = table.Column<string>(type: "text", nullable: false),
                    ce_traceparent = table.Column<string>(type: "text", nullable: false),
                    ce_tracestate = table.Column<string>(type: "text", nullable: true),
                    ce_conversationid = table.Column<string>(type: "text", nullable: true),
                    ce_tenant = table.Column<string>(type: "text", nullable: true),
                    payload = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    destination_name = table.Column<string>(type: "text", nullable: false),
                    subscription_name = table.Column<string>(type: "text", nullable: true),
                    op_name = table.Column<string>(type: "text", nullable: false, defaultValue: "send"),
                    op_type = table.Column<string>(type: "text", nullable: false, defaultValue: "send"),
                    server_address = table.Column<string>(type: "text", nullable: true),
                    server_port = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_outbox_cetype",
                table: "outbox",
                column: "ce_type");

            migrationBuilder.CreateIndex(
                name: "idx_outbox_conversation",
                table: "outbox",
                column: "ce_conversationid");

            migrationBuilder.CreateIndex(
                name: "idx_outbox_destination",
                table: "outbox",
                column: "destination_name");

            migrationBuilder.CreateIndex(
                name: "idx_outbox_status_created_at",
                table: "outbox",
                columns: new[] { "status", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox");
        }
    }
}
