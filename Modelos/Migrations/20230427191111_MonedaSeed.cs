using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class MonedaSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Monedas",
                columns: new[] { "Nombre", "Descripcion", "Abreviatura", "IdUsuario" },
                values: new object[,]
                {
                    { "Bolviano", "Bolviano", "BOB", 1 },
                    { "Dolar", "Dolar Estadounidense", "USD", 1 },
                    { "Euro", "Euro europeo", "EUR", 1 },
                    { "GBP", "Libra esterlina", "GBP", 1 },
                    { "Real", "Real brasilero", "BRL", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }


    }
}