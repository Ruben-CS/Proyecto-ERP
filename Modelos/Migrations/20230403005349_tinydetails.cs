using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class tinydetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gestiones_Empresas_IdEmpresa",
                table: "Gestiones");

            migrationBuilder.DropIndex(
                name: "IX_Gestiones_IdEmpresa",
                table: "Gestiones");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaDtoIdEmpresa",
                table: "Gestiones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_EmpresaDtoIdEmpresa",
                table: "Gestiones",
                column: "EmpresaDtoIdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Gestiones_Empresas_EmpresaDtoIdEmpresa",
                table: "Gestiones",
                column: "EmpresaDtoIdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gestiones_Empresas_EmpresaDtoIdEmpresa",
                table: "Gestiones");

            migrationBuilder.DropIndex(
                name: "IX_Gestiones_EmpresaDtoIdEmpresa",
                table: "Gestiones");

            migrationBuilder.DropColumn(
                name: "EmpresaDtoIdEmpresa",
                table: "Gestiones");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_IdEmpresa",
                table: "Gestiones",
                column: "IdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Gestiones_Empresas_IdEmpresa",
                table: "Gestiones",
                column: "IdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
