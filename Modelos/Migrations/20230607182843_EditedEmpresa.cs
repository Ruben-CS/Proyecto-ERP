using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class EditedEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cuenta1",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cuenta2",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cuenta3",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cuenta4",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cuenta5",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cuenta6",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cuenta7",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneIntegracion",
                table: "Empresas",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cuenta1",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cuenta2",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cuenta3",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cuenta4",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cuenta5",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cuenta6",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cuenta7",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "TieneIntegracion",
                table: "Empresas");
        }
    }
}
