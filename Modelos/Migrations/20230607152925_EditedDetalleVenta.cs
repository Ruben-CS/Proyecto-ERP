using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class EditedDetalleVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Articulo_IdArticulo",
                table: "Detalle");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Lotes_NroLote_IdArticulo",
                table: "Detalle");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Nota_IdNota",
                table: "Detalle");

            migrationBuilder.DropIndex(
                name: "IX_Detalle_NroLote_IdArticulo",
                table: "Detalle");

            migrationBuilder.AlterColumn<int>(
                name: "IdNota",
                table: "Detalle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "NroLote",
                table: "Detalle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "IdArticulo",
                table: "Detalle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "IdDetalleVenta",
                table: "Detalle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Articulo_IdArticulo",
                table: "Detalle",
                column: "IdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Nota_IdNota",
                table: "Detalle",
                column: "IdNota",
                principalTable: "Nota",
                principalColumn: "IdNota",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Articulo_IdArticulo",
                table: "Detalle");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Nota_IdNota",
                table: "Detalle");

            migrationBuilder.DropColumn(
                name: "IdDetalleVenta",
                table: "Detalle");

            migrationBuilder.AlterColumn<int>(
                name: "IdNota",
                table: "Detalle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "NroLote",
                table: "Detalle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "IdArticulo",
                table: "Detalle",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_NroLote_IdArticulo",
                table: "Detalle",
                columns: new[] { "NroLote", "IdArticulo" });

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Articulo_IdArticulo",
                table: "Detalle",
                column: "IdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Lotes_NroLote_IdArticulo",
                table: "Detalle",
                columns: new[] { "NroLote", "IdArticulo" },
                principalTable: "Lotes",
                principalColumns: new[] { "IdLote", "IdArticulo" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Nota_IdNota",
                table: "Detalle",
                column: "IdNota",
                principalTable: "Nota",
                principalColumn: "IdNota",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
