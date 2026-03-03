using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lat = table.Column<double>(type: "double precision", nullable: false),
                    lon = table.Column<double>(type: "double precision", nullable: false),
                    city_name = table.Column<string>(type: "text", nullable: true),
                    resolved_city_name = table.Column<string>(type: "text", nullable: false),
                    country_code = table.Column<string>(type: "text", nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stat_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_name = table.Column<string>(type: "text", nullable: false),
                    country_code = table.Column<string>(type: "text", nullable: false),
                    lat = table.Column<double>(type: "double precision", nullable: false),
                    lon = table.Column<double>(type: "double precision", nullable: false),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stat_entries", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_entries_requested_at",
                table: "audit_entries",
                column: "requested_at");

            migrationBuilder.CreateIndex(
                name: "IX_audit_entries_resolved_city_name",
                table: "audit_entries",
                column: "resolved_city_name");

            migrationBuilder.CreateIndex(
                name: "IX_stat_entries_city_name",
                table: "stat_entries",
                column: "city_name");

            migrationBuilder.CreateIndex(
                name: "IX_stat_entries_country_code",
                table: "stat_entries",
                column: "country_code");

            migrationBuilder.CreateIndex(
                name: "IX_stat_entries_requested_at",
                table: "stat_entries",
                column: "requested_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_entries");

            migrationBuilder.DropTable(
                name: "stat_entries");
        }
    }
}
