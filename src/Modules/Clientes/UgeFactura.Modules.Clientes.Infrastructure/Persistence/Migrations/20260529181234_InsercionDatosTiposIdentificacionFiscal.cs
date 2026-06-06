using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UgeFactura.Modules.Clientes.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InsercionDatosTiposIdentificacionFiscal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "clientes",
                table: "tipos_identificacion_fiscal",
                columns: new[] { "Codigo", "nombre", "regla", "requiere_pais" },
                values: new object[,]
                {
                    { "02", "NIF-IVA", "NifNieCif", false },
                    { "03", "Pasaporte", "NifNieCif", false },
                    { "04", "Documento oficial de identificación expedido por el país o territorio de residencia", "NifNieCif", false },
                    { "05", "Certificado de residencia", "NifNieCif", false },
                    { "06", "Otro documento probatorio", "NifNieCif", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "tipos_identificacion_fiscal",
                keyColumn: "Codigo",
                keyValue: "02");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "tipos_identificacion_fiscal",
                keyColumn: "Codigo",
                keyValue: "03");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "tipos_identificacion_fiscal",
                keyColumn: "Codigo",
                keyValue: "04");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "tipos_identificacion_fiscal",
                keyColumn: "Codigo",
                keyValue: "05");

            migrationBuilder.DeleteData(
                schema: "clientes",
                table: "tipos_identificacion_fiscal",
                keyColumn: "Codigo",
                keyValue: "06");
        }
    }
}
