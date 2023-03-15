using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModuloContabilidadApi.Migrations
{
    /// <inheritdoc />
    public partial class EditadoLaEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Empresas",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Empresas");
        }
    }
}
