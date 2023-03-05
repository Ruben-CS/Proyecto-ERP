using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModuloContabilidadApi.Migrations
{
    /// <inheritdoc />
    public partial class ModificacionDeRelacionPeriodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gestiones_Empresas_EmpresaIdEmpresa",
                table: "Gestiones");

            migrationBuilder.DropForeignKey(
                name: "FK_Periodos_Gestiones_GestionIdGestion",
                table: "Periodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Periodos_Usuarios_UsuarioIdUsuario",
                table: "Periodos");

            migrationBuilder.DropIndex(
                name: "IX_Periodos_GestionIdGestion",
                table: "Periodos");

            migrationBuilder.DropIndex(
                name: "IX_Periodos_UsuarioIdUsuario",
                table: "Periodos");

            migrationBuilder.DropIndex(
                name: "IX_Gestiones_EmpresaIdEmpresa",
                table: "Gestiones");

            migrationBuilder.DropColumn(
                name: "GestionIdGestion",
                table: "Periodos");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "Periodos");

            migrationBuilder.DropColumn(
                name: "EmpresaIdEmpresa",
                table: "Gestiones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GestionIdGestion",
                table: "Periodos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioIdUsuario",
                table: "Periodos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaIdEmpresa",
                table: "Gestiones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periodos_GestionIdGestion",
                table: "Periodos",
                column: "GestionIdGestion");

            migrationBuilder.CreateIndex(
                name: "IX_Periodos_UsuarioIdUsuario",
                table: "Periodos",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_EmpresaIdEmpresa",
                table: "Gestiones",
                column: "EmpresaIdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Gestiones_Empresas_EmpresaIdEmpresa",
                table: "Gestiones",
                column: "EmpresaIdEmpresa",
                principalTable: "Empresas",
                principalColumn: "IdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Periodos_Gestiones_GestionIdGestion",
                table: "Periodos",
                column: "GestionIdGestion",
                principalTable: "Gestiones",
                principalColumn: "IdGestion");

            migrationBuilder.AddForeignKey(
                name: "FK_Periodos_Usuarios_UsuarioIdUsuario",
                table: "Periodos",
                column: "UsuarioIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");
        }
    }
}
