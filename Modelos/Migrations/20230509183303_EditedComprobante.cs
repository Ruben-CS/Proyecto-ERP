using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class EditedComprobante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TC",
                table: "Comprobantes",
                newName: "Tc");

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Comprobantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_IdEmpresa",
                table: "Comprobantes",
                column: "IdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Comprobantes_Empresas_IdEmpresa",
                table: "Comprobantes",
                column: "IdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comprobantes_Empresas_IdEmpresa",
                table: "Comprobantes");

            migrationBuilder.DropIndex(
                name: "IX_Comprobantes_IdEmpresa",
                table: "Comprobantes");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Comprobantes");

            migrationBuilder.RenameColumn(
                name: "Tc",
                table: "Comprobantes",
                newName: "TC");
        }
    }
}
