using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturity.Modules.Auth.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn_TenantId_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                schema: "auth",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_tenant_id",
                schema: "auth",
                table: "AspNetUsers",
                column: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_tenant_id",
                schema: "auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                schema: "auth",
                table: "AspNetUsers");
        }
    }
}
