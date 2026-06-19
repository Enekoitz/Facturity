using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UgeFactura.Modules.Clientes.Infrastructure
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "clientes");

            migrationBuilder.CreateTable(
                name: "tipos_identificacion_fiscal",
                schema: "clientes",
                columns: table => new
                {
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    regla = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    requiere_pais = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                schema: "clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Identificacion_tipo_identificacion_fiscal_id = table.Column<string>(type: "text", nullable: false),
                    identificacion_fiscal_valor = table.Column<string>(type: "text", nullable: false),
                    identificacion_pais = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    primer_apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    segundo_apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    domicilio_direccion = table.Column<string>(type: "text", nullable: false),
                    domicilio_codigo_postal = table.Column<string>(type: "text", nullable: true),
                    domicilio_municipio = table.Column<string>(type: "text", nullable: true),
                    domicilio_provincia = table.Column<string>(type: "text", nullable: true),
                    Domicilio_Region = table.Column<string>(type: "text", nullable: true),
                    domicilio_pais = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.Id);
                    table.ForeignKey(
                        name: "fk_cliente_tipo_identificacion",
                        column: x => x.Identificacion_tipo_identificacion_fiscal_id,
                        principalSchema: "clientes",
                        principalTable: "tipos_identificacion_fiscal",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientes_Identificacion_tipo_identificacion_fiscal_id",
                schema: "clientes",
                table: "clientes",
                column: "Identificacion_tipo_identificacion_fiscal_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clientes",
                schema: "clientes");

            migrationBuilder.DropTable(
                name: "tipos_identificacion_fiscal",
                schema: "clientes");
        }
    }
}
