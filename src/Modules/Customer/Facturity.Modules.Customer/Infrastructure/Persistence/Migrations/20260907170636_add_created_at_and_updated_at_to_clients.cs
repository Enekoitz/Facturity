using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturity.Modules.Customer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_created_at_and_updated_at_to_clients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at_utc",
                schema: "clientes",
                table: "clientes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                schema: "clientes",
                table: "clientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at_utc",
                schema: "clientes",
                table: "clientes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                schema: "clientes",
                table: "clientes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "updated_by",
                schema: "clientes",
                table: "clientes");
        }
    }
}
