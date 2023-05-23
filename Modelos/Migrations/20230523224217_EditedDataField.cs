using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class EditedDataField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticuloCategoria_Articulo_IdArticulo",
                table: "ArticuloCategoria");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticuloCategoria_Categoria_IdCategoria",
                table: "ArticuloCategoria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticuloCategoria",
                table: "ArticuloCategoria");

            migrationBuilder.RenameTable(
                name: "ArticuloCategoria",
                newName: "ArticuloCategoriaa");

            migrationBuilder.RenameIndex(
                name: "IX_ArticuloCategoria_IdCategoria",
                table: "ArticuloCategoriaa",
                newName: "IX_ArticuloCategoriaa_IdCategoria");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioVenta",
                table: "Articulo",
                type: "decimal(18,4",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticuloCategoriaa",
                table: "ArticuloCategoriaa",
                columns: new[] { "IdArticulo", "IdCategoria" });

            migrationBuilder.AddForeignKey(
                name: "FK_ArticuloCategoriaa_Articulo_IdArticulo",
                table: "ArticuloCategoriaa",
                column: "IdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticuloCategoriaa_Categoria_IdCategoria",
                table: "ArticuloCategoriaa",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticuloCategoriaa_Articulo_IdArticulo",
                table: "ArticuloCategoriaa");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticuloCategoriaa_Categoria_IdCategoria",
                table: "ArticuloCategoriaa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticuloCategoriaa",
                table: "ArticuloCategoriaa");

            migrationBuilder.RenameTable(
                name: "ArticuloCategoriaa",
                newName: "ArticuloCategoria");

            migrationBuilder.RenameIndex(
                name: "IX_ArticuloCategoriaa_IdCategoria",
                table: "ArticuloCategoria",
                newName: "IX_ArticuloCategoria_IdCategoria");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioVenta",
                table: "Articulo",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticuloCategoria",
                table: "ArticuloCategoria",
                columns: new[] { "IdArticulo", "IdCategoria" });

            migrationBuilder.AddForeignKey(
                name: "FK_ArticuloCategoria_Articulo_IdArticulo",
                table: "ArticuloCategoria",
                column: "IdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticuloCategoria_Categoria_IdCategoria",
                table: "ArticuloCategoria",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
