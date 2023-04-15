using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class EditedEssentialFieldToCuenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadreIdCuenta",
                table: "Cuentas");

            migrationBuilder.RenameColumn(
                name: "IdCuentaPadreIdCuenta",
                table: "Cuentas",
                newName: "IdCuentaPadreNavigationIdCuenta");

            migrationBuilder.RenameIndex(
                name: "IX_Cuentas_IdCuentaPadreIdCuenta",
                table: "Cuentas",
                newName: "IX_Cuentas_IdCuentaPadreNavigationIdCuenta");

            migrationBuilder.AddColumn<int>(
                name: "IdCuentaPadre",
                table: "Cuentas",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas",
                column: "IdCuentaPadreNavigationIdCuenta",
                principalTable: "Cuentas",
                principalColumn: "IdCuenta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "IdCuentaPadre",
                table: "Cuentas");

            migrationBuilder.RenameColumn(
                name: "IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas",
                newName: "IdCuentaPadreIdCuenta");

            migrationBuilder.RenameIndex(
                name: "IX_Cuentas_IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas",
                newName: "IX_Cuentas_IdCuentaPadreIdCuenta");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadreIdCuenta",
                table: "Cuentas",
                column: "IdCuentaPadreIdCuenta",
                principalTable: "Cuentas",
                principalColumn: "IdCuenta");
        }
    }
}
