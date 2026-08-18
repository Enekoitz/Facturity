using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Facturity.Modules.Customer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "clientes");

            migrationBuilder.CreateTable(
                name: "catalogo_tipos_identificacion_fiscal",
                schema: "clientes",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    nombre_completo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    codigo_ticketbai_bizkaia = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogo_tipos_identificacion_fiscal", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                schema: "clientes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    identificacion_fiscal_valor = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    identificacion_fiscal_tipo = table.Column<int>(type: "smallint", nullable: false),
                    identificacion_fiscal_codigo_pais = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    primer_apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    segundo_apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    domicilio_direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    domicilio_codigo_postal = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    domicilio_municipio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    domicilio_provincia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    domicilio_region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    domicilio_pais = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                columns: new[] { "codigo", "codigo_ticketbai_bizkaia", "Id", "nombre", "nombre_completo" },
                values: new object[,]
                {
                    { "CER_RES", "05", 5, "Certificado de residencia", "Certificado de residencia" },
                    { "DOC_PAI", "04", 4, "Documento oficial de identificación expedido por el país o territorio de residencia", "Documento oficial de identificación expedido por el país o territorio de residencia" },
                    { "NIF", "01", 1, "NIF", "Número de identificación fiscal" },
                    { "NIF_IVA", "02", 2, "NIF-IVA", "NIF-IVA Comunitario" },
                    { "OTR_DOC_PRO", "06", 6, "Otro documento probatorio", "Otro documento probatorio" },
                    { "PAS", "03", 3, "Pasaporte", "Pasaporte" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalogo_tipos_identificacion_fiscal",
                schema: "clientes");

            migrationBuilder.DropTable(
                name: "clientes",
                schema: "clientes");
        }
    }
}
