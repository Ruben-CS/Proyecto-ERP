using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class tryFixConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periodos_Gestiones_IdUsuario",
                table: "Periodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Periodos_Usuarios_IdGestion",
                table: "Periodos");

            migrationBuilder.AddForeignKey(
                name: "FK_Periodos_Gestiones_IdGestion",
                table: "Periodos",
                column: "IdGestion",
                principalTable: "Gestiones",
                principalColumn: "IdGestion",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Periodos_Usuarios_IdUsuario",
                table: "Periodos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periodos_Gestiones_IdGestion",
                table: "Periodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Periodos_Usuarios_IdUsuario",
                table: "Periodos");

            migrationBuilder.AddForeignKey(
                name: "FK_Periodos_Gestiones_IdUsuario",
                table: "Periodos",
                column: "IdUsuario",
                principalTable: "Gestiones",
                principalColumn: "IdGestion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Periodos_Usuarios_IdGestion",
                table: "Periodos",
                column: "IdGestion",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
