using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "event_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_messages", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_entries_received_at",
                table: "audit_entries",
                column: "received_at");

            migrationBuilder.CreateIndex(
                name: "IX_event_messages_created_at",
                table: "event_messages",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_event_messages_processed_at",
                table: "event_messages",
                column: "processed_at");

            migrationBuilder.CreateIndex(
                name: "IX_event_messages_processed_at_created_at",
                table: "event_messages",
                columns: new[] { "processed_at", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_messages");

            migrationBuilder.DropIndex(
                name: "IX_audit_entries_received_at",
                table: "audit_entries");
        }
    }
}
