using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class nombrecuentaadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreCuenta",
                table: "DetalleComprobantes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreCuenta",
                table: "DetalleComprobantes");
        }
    }
}
