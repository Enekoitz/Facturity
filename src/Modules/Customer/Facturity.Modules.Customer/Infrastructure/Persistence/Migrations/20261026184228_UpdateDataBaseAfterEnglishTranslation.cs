using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Facturity.Modules.Customer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataBaseAfterEnglishTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientes_catalogo_tipos_identificacion_fiscal_identificacio~",
                schema: "customers",
                table: "customers");

            migrationBuilder.DropTable(
                name: "catalogo_tipos_identificacion_fiscal",
                schema: "customers");

            migrationBuilder.EnsureSchema(
                name: "customer");

            migrationBuilder.RenameTable(
                name: "customers",
                schema: "customers",
                newName: "customers",
                newSchema: "customer");

            migrationBuilder.RenameColumn(
                name: "segundo_apellido",
                schema: "customer",
                table: "customers",
                newName: "second_surname");

            migrationBuilder.RenameColumn(
                name: "razon_social",
                schema: "customer",
                table: "customers",
                newName: "legal_entity_name");

            migrationBuilder.RenameColumn(
                name: "primer_apellido",
                schema: "customer",
                table: "customers",
                newName: "first_surname");

            migrationBuilder.RenameColumn(
                name: "nombre",
                schema: "customer",
                table: "customers",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "identificacion_fiscal_valor",
                schema: "customer",
                table: "customers",
                newName: "tax_id_value");

            migrationBuilder.RenameColumn(
                name: "identificacion_fiscal_tipo",
                schema: "customer",
                table: "customers",
                newName: "tax_id_type");

            migrationBuilder.RenameColumn(
                name: "identificacion_fiscal_codigo_pais",
                schema: "customer",
                table: "customers",
                newName: "tax_id_country_code");

            migrationBuilder.RenameColumn(
                name: "domicilio_region",
                schema: "customer",
                table: "customers",
                newName: "address_region");

            migrationBuilder.RenameColumn(
                name: "domicilio_provincia",
                schema: "customer",
                table: "customers",
                newName: "address_state");

            migrationBuilder.RenameColumn(
                name: "domicilio_pais",
                schema: "customer",
                table: "customers",
                newName: "address_country");

            migrationBuilder.RenameColumn(
                name: "domicilio_municipio",
                schema: "customer",
                table: "customers",
                newName: "address_city");

            migrationBuilder.RenameColumn(
                name: "domicilio_codigo_postal",
                schema: "customer",
                table: "customers",
                newName: "address_postal_code");

            migrationBuilder.RenameColumn(
                name: "direccion_busqueda",
                schema: "customer",
                table: "customers",
                newName: "address_search");

            migrationBuilder.RenameColumn(
                name: "domicilio_direccion",
                schema: "customer",
                table: "customers",
                newName: "address_street");

            migrationBuilder.RenameIndex(
                name: "IX_clientes_identificacion_fiscal_tipo",
                schema: "customer",
                table: "customers",
                newName: "IX_customers_tax_id_type");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                schema: "customer",
                table: "customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at_utc",
                schema: "customer",
                table: "customers",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                schema: "customer",
                table: "customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at_utc",
                schema: "customer",
                table: "customers",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "denominacion_fiscal_busqueda",
                schema: "customer",
                table: "customers",
                type: "text",
                nullable: false,
                computedColumnSql: "CASE WHEN legal_entity_name IS NOT NULL THEN legal_entity_name ELSE TRIM(COALESCE(first_name, '') || ' ' || COALESCE(first_surname, '') || ' ' || COALESCE(second_surname, '')) END",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "CASE WHEN razon_social IS NOT NULL THEN razon_social ELSE TRIM(COALESCE(nombre, '') || ' ' || COALESCE(primer_apellido, '') || ' ' || COALESCE(segundo_apellido, '')) END",
                oldStored: true);

            migrationBuilder.AlterColumn<string>(
                name: "address_search",
                schema: "customer",
                table: "customers",
                type: "text",
                nullable: false,
                computedColumnSql: "COALESCE(address_street, '') || ', ' || COALESCE(address_city, '') || ', ' || COALESCE(address_postal_code, '') || ', ' || COALESCE(address_state, '') || ', ' || COALESCE(address_region, '') || ', ' || COALESCE(address_country, '')",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "COALESCE(domicilio_direccion, '') || ', ' || COALESCE(domicilio_municipio, '') || ', ' || COALESCE(domicilio_codigo_postal, '') || ', ' || COALESCE(domicilio_provincia, '') || ', ' || COALESCE(domicilio_region, '') || ', ' || COALESCE(domicilio_pais, '')",
                oldStored: true);

            migrationBuilder.CreateTable(
                name: "tax_id_types_catalog",
                schema: "customer",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    code_ticket_bai_bizkaia = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tax_id_types_catalog", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "customer",
                table: "tax_id_types_catalog",
                columns: new[] { "id", "code", "full_name", "name", "code_ticket_bai_bizkaia" },
                values: new object[,]
                {
                    { (short)1, "SP_TAX_ID", "Spanish Tax ID", "Spanish Tax ID", "01" },
                    { (short)2, "INT_COM_VAT_ID", "Intra-community VAT ID", "Intra-community VAT ID", "02" },
                    { (short)3, "PAS", "Passport", "Passport", "03" },
                    { (short)4, "ORIGIN_ID_DOC", "Official identification document issued by the country or territory of residence", "Official identification document issued by the country or territory of residence", "04" },
                    { (short)5, "RES_CERT", "Residence Certificate", "Residence Certificate", "05" },
                    { (short)6, "OTH_SUPP_DOC", "Other supporting document", "Other supporting document", "06" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_customers_tax_id_types_catalog_tax_id_type",
                schema: "customer",
                table: "customers",
                column: "tax_id_type",
                principalSchema: "customer",
                principalTable: "tax_id_types_catalog",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customers_tax_id_types_catalog_tax_id_type",
                schema: "customer",
                table: "customers");

            migrationBuilder.DropTable(
                name: "tax_id_types_catalog",
                schema: "customer");

            migrationBuilder.EnsureSchema(
                name: "customers");

            migrationBuilder.RenameTable(
                name: "customers",
                schema: "customer",
                newName: "customers",
                newSchema: "customers");

            migrationBuilder.RenameColumn(
                name: "tax_id_value",
                schema: "customers",
                table: "customers",
                newName: "identificacion_fiscal_valor");

            migrationBuilder.RenameColumn(
                name: "tax_id_type",
                schema: "customers",
                table: "customers",
                newName: "identificacion_fiscal_tipo");

            migrationBuilder.RenameColumn(
                name: "tax_id_country_code",
                schema: "customers",
                table: "customers",
                newName: "identificacion_fiscal_codigo_pais");

            migrationBuilder.RenameColumn(
                name: "second_surname",
                schema: "customers",
                table: "customers",
                newName: "segundo_apellido");

            migrationBuilder.RenameColumn(
                name: "legal_entity_name",
                schema: "customers",
                table: "customers",
                newName: "razon_social");

            migrationBuilder.RenameColumn(
                name: "first_surname",
                schema: "customers",
                table: "customers",
                newName: "primer_apellido");

            migrationBuilder.RenameColumn(
                name: "first_name",
                schema: "customers",
                table: "customers",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "address_state",
                schema: "customers",
                table: "customers",
                newName: "domicilio_provincia");

            migrationBuilder.RenameColumn(
                name: "address_search",
                schema: "customers",
                table: "customers",
                newName: "direccion_busqueda");

            migrationBuilder.RenameColumn(
                name: "address_region",
                schema: "customers",
                table: "customers",
                newName: "domicilio_region");

            migrationBuilder.RenameColumn(
                name: "address_postal_code",
                schema: "customers",
                table: "customers",
                newName: "domicilio_codigo_postal");

            migrationBuilder.RenameColumn(
                name: "address_country",
                schema: "customers",
                table: "customers",
                newName: "domicilio_pais");

            migrationBuilder.RenameColumn(
                name: "address_city",
                schema: "customers",
                table: "customers",
                newName: "domicilio_municipio");

            migrationBuilder.RenameColumn(
                name: "address_street",
                schema: "customers",
                table: "customers",
                newName: "domicilio_direccion");

            migrationBuilder.RenameIndex(
                name: "IX_customers_tax_id_type",
                schema: "customers",
                table: "customers",
                newName: "IX_clientes_identificacion_fiscal_tipo");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                schema: "customers",
                table: "customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at_utc",
                schema: "customers",
                table: "customers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                schema: "customers",
                table: "customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at_utc",
                schema: "customers",
                table: "customers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<string>(
                name: "denominacion_fiscal_busqueda",
                schema: "customers",
                table: "customers",
                type: "text",
                nullable: false,
                computedColumnSql: "CASE WHEN razon_social IS NOT NULL THEN razon_social ELSE TRIM(COALESCE(nombre, '') || ' ' || COALESCE(primer_apellido, '') || ' ' || COALESCE(segundo_apellido, '')) END",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "CASE WHEN legal_entity_name IS NOT NULL THEN legal_entity_name ELSE TRIM(COALESCE(first_name, '') || ' ' || COALESCE(first_surname, '') || ' ' || COALESCE(second_surname, '')) END",
                oldStored: true);

            migrationBuilder.AlterColumn<string>(
                name: "direccion_busqueda",
                schema: "customers",
                table: "customers",
                type: "text",
                nullable: false,
                computedColumnSql: "COALESCE(domicilio_direccion, '') || ', ' || COALESCE(domicilio_municipio, '') || ', ' || COALESCE(domicilio_codigo_postal, '') || ', ' || COALESCE(domicilio_provincia, '') || ', ' || COALESCE(domicilio_region, '') || ', ' || COALESCE(domicilio_pais, '')",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "COALESCE(address_street, '') || ', ' || COALESCE(address_city, '') || ', ' || COALESCE(address_postal_code, '') || ', ' || COALESCE(address_state, '') || ', ' || COALESCE(address_region, '') || ', ' || COALESCE(address_country, '')",
                oldStored: true);

            migrationBuilder.CreateTable(
                name: "catalogo_tipos_identificacion_fiscal",
                schema: "customers",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false),
                    codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    codigo_ticketbai_bizkaia = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nombre_completo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogo_tipos_identificacion_fiscal", x => x.id);
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

            migrationBuilder.AddForeignKey(
                name: "FK_clientes_catalogo_tipos_identificacion_fiscal_identificacio~",
                schema: "customers",
                table: "customers",
                column: "identificacion_fiscal_tipo",
                principalSchema: "customers",
                principalTable: "catalogo_tipos_identificacion_fiscal",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
