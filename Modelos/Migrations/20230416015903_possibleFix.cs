using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class possibleFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Empresas_IdCuentaPadre",
                table: "Cuentas");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdEmpresa",
                table: "Cuentas",
                column: "IdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Empresas_IdEmpresa",
                table: "Cuentas",
                column: "IdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Empresas_IdEmpresa",
                table: "Cuentas");

            migrationBuilder.DropIndex(
                name: "IX_Cuentas_IdEmpresa",
                table: "Cuentas");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Empresas_IdCuentaPadre",
                table: "Cuentas",
                column: "IdCuentaPadre",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa");
        }
    }
}
