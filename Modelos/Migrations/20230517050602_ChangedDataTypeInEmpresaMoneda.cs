using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDataTypeInEmpresaMoneda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TipoComprobante",
                table: "Comprobantes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoComprobante",
                table: "Comprobantes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
