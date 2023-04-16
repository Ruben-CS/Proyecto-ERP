using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRelationShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Empresas_IdUsuario",
                table: "Cuentas");

            migrationBuilder.DropIndex(
                name: "IX_Cuentas_IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas");

            migrationBuilder.DropIndex(
                name: "IX_Cuentas_IdUsuario",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdCuentaPadre",
                table: "Cuentas",
                column: "IdCuentaPadre");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadre",
                table: "Cuentas",
                column: "IdCuentaPadre",
                principalTable: "Cuentas",
                principalColumn: "IdCuenta");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Empresas_IdCuentaPadre",
                table: "Cuentas",
                column: "IdCuentaPadre",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadre",
                table: "Cuentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Empresas_IdCuentaPadre",
                table: "Cuentas");

            migrationBuilder.DropIndex(
                name: "IX_Cuentas_IdCuentaPadre",
                table: "Cuentas");

            migrationBuilder.AddColumn<int>(
                name: "IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas",
                column: "IdCuentaPadreNavigationIdCuenta");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdUsuario",
                table: "Cuentas",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Cuentas_IdCuentaPadreNavigationIdCuenta",
                table: "Cuentas",
                column: "IdCuentaPadreNavigationIdCuenta",
                principalTable: "Cuentas",
                principalColumn: "IdCuenta");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Empresas_IdUsuario",
                table: "Cuentas",
                column: "IdUsuario",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
