using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UgeFactura.Modules.Clientes.Infrastructure
{
    /// <inheritdoc />
    public partial class Clientes_UpdateColumnName_Domicilio_Region : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Domicilio_Region",
                schema: "clientes",
                table: "clientes",
                newName: "domicilio_region");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "domicilio_region",
                schema: "clientes",
                table: "clientes",
                newName: "Domicilio_Region");
        }
    }
}
