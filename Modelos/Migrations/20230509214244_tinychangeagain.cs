using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class tinychangeagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comprobantes_Empresas_IdEmpresa",
                table: "Comprobantes");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Comprobantes",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddForeignKey(
                name: "FK_Comprobantes_Empresas_IdEmpresa",
                table: "Comprobantes",
                column: "IdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comprobantes_Empresas_IdEmpresa",
                table: "Comprobantes");

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Comprobantes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comprobantes_Empresas_IdEmpresa",
                table: "Comprobantes",
                column: "IdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
