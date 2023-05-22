using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class EditedTipoCuentoToEnumType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MontoHaberAlt",
                table: "DetalleComprobantes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoHaber",
                table: "DetalleComprobantes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoDebeAlt",
                table: "DetalleComprobantes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoDebe",
                table: "DetalleComprobantes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "MontoHaberAlt",
                table: "DetalleComprobantes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<float>(
                name: "MontoHaber",
                table: "DetalleComprobantes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<float>(
                name: "MontoDebeAlt",
                table: "DetalleComprobantes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<float>(
                name: "MontoDebe",
                table: "DetalleComprobantes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
