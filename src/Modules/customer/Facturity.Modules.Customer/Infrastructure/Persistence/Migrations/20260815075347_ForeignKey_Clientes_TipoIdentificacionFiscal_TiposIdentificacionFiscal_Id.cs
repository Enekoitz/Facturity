using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Facturity.Modules.Customer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey_Clientes_TipoIdentificacionFiscal_TiposIdentificacionFiscal_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_catalogo_tipos_identificacion_fiscal",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "codigo",
                keyValue: "CER_RES");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "codigo",
                keyValue: "DOC_PAI");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "codigo",
                keyValue: "NIF");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "codigo",
                keyValue: "NIF_IVA");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "codigo",
                keyValue: "OTR_DOC_PRO");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "codigo",
                keyValue: "PAS");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "nombre_completo",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<short>(
                name: "id",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_catalogo_tipos_identificacion_fiscal",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                column: "id");

            migrationBuilder.InsertData(
                schema: "clientes",
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
                schema: "clientes",
                table: "clientes",
                column: "identificacion_fiscal_tipo");

            migrationBuilder.AddForeignKey(
                name: "FK_clientes_catalogo_tipos_identificacion_fiscal_identificacio~",
                schema: "clientes",
                table: "clientes",
                column: "identificacion_fiscal_tipo",
                principalSchema: "clientes",
                principalTable: "catalogo_tipos_identificacion_fiscal",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientes_catalogo_tipos_identificacion_fiscal_identificacio~",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "IX_clientes_identificacion_fiscal_tipo",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_catalogo_tipos_identificacion_fiscal",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "id",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "id",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "id",
                keyValue: (short)5);

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                keyColumn: "id",
                keyValue: (short)6);

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "nombre_completo",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_catalogo_tipos_identificacion_fiscal",
                schema: "clientes",
                table: "catalogo_tipos_identificacion_fiscal",
                column: "codigo");

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
    }
}
