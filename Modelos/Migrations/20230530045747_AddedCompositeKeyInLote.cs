using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompositeKeyInLote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Lotes",
                table: "Lotes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioCompra",
                table: "Lotes",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lotes",
                table: "Lotes",
                columns: new[] { "IdLote", "IdArticulo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Lotes",
                table: "Lotes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioCompra",
                table: "Lotes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lotes",
                table: "Lotes",
                column: "IdLote");
        }
    }
}
