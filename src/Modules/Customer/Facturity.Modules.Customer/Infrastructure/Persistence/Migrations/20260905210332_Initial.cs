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
                name: "customers");

            migrationBuilder.CreateTable(
                name: "catalogo_tipos_identificacion_fiscal",
                schema: "customers",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false),
                    codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nombre_completo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    codigo_ticketbai_bizkaia = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogo_tipos_identificacion_fiscal", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    identificacion_fiscal_valor = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    identificacion_fiscal_tipo = table.Column<short>(type: "smallint", nullable: false),
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
                    domicilio_pais = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    activated_at_utc = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    deactivated_at_utc = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    deleted_at_utc = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    direccion_busqueda = table.Column<string>(type: "text", nullable: false, computedColumnSql: "COALESCE(domicilio_direccion, '') || ', ' || COALESCE(domicilio_municipio, '') || ', ' || COALESCE(domicilio_codigo_postal, '') || ', ' || COALESCE(domicilio_provincia, '') || ', ' || COALESCE(domicilio_region, '') || ', ' || COALESCE(domicilio_pais, '')", stored: true),
                    denominacion_fiscal_busqueda = table.Column<string>(type: "text", nullable: false, computedColumnSql: "CASE WHEN razon_social IS NOT NULL THEN razon_social ELSE TRIM(COALESCE(nombre, '') || ' ' || COALESCE(primer_apellido, '') || ' ' || COALESCE(segundo_apellido, '')) END", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_clientes_catalogo_tipos_identificacion_fiscal_identificacio~",
                        column: x => x.identificacion_fiscal_tipo,
                        principalSchema: "customers",
                        principalTable: "catalogo_tipos_identificacion_fiscal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "catalogo_tipos_identificacion_fiscal",
                columns: new[] { "id", "codigo", "codigo_ticketbai_bizkaia", "nombre", "nombre_completo" },
                values: new object[,]
                {
                    { (short)1, "NIF", "01", "NIF", "Número de identificación fiscal" },
                    { (short)2, "NIF_IVA", "02", "NIF-IVA", "NIF-IVA Comunitario" },
                    { (short)3, "PAS", "03", "Pasaporte", "Pasaporte" },
                    { (short)4, "DOC_PAI", "04", "Documento oficial de identificación expedido por el país o territorio de residencia", "Documento oficial de identificación expedido por el país o territorio de residencia" },
                    { (short)5, "CER_RES", "05", "Certificado de residencia", "Certificado de residencia" },
                    { (short)6, "OTR_DOC_PRO", "06", "Otro documento probatorio", "Otro documento probatorio" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientes_identificacion_fiscal_tipo",
                schema: "customers",
                table: "customers",
                column: "identificacion_fiscal_tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "catalogo_tipos_identificacion_fiscal",
                schema: "customers");
        }
    }
}
