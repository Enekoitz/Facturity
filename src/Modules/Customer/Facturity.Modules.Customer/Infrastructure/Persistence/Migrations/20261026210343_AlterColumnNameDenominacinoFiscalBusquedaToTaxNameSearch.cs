using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturity.Modules.Customer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnNameDenominacinoFiscalBusquedaToTaxNameSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "denominacion_fiscal_busqueda",
                schema: "customer",
                table: "customers",
                newName: "tax_name_search");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tax_name_search",
                schema: "customer",
                table: "customers",
                newName: "denominacion_fiscal_busqueda");
        }
    }
}
