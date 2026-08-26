using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturity.Modules.Customer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AñadircampoDeletedAtUtcalaentidadClienteRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at_utc",
                schema: "clientes",
                table: "clientes",
                type: "timestamptz",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at_utc",
                schema: "clientes",
                table: "clientes");
        }
    }
}
