using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturity.Modules.Customer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AñadircolumnasActivatedAtUtcyDeactivatedAtUtcenlaentidadCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "activated_at_utc",
                schema: "clientes",
                table: "clientes",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deactivated_at_utc",
                schema: "clientes",
                table: "clientes",
                type: "timestamptz",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activated_at_utc",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "deactivated_at_utc",
                schema: "clientes",
                table: "clientes");
        }
    }
}
