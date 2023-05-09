using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class AddedRemainingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comprobantes",
                columns: table => new
                {
                    IdComprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Serie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Glosa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    TipoComprobante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdMoneda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobantes", x => x.IdComprobante);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleComprobantes",
                columns: table => new
                {
                    IdDetalleComprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Glosa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoDebe = table.Column<float>(type: "real", nullable: false),
                    MontoHaber = table.Column<float>(type: "real", nullable: false),
                    MontoDebeAlt = table.Column<float>(type: "real", nullable: false),
                    MontoHaberAlt = table.Column<float>(type: "real", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdComprobante = table.Column<int>(type: "int", nullable: false),
                    IdCuenta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleComprobantes", x => x.IdDetalleComprobante);
                    table.ForeignKey(
                        name: "FK_DetalleComprobantes_Comprobantes_IdComprobante",
                        column: x => x.IdComprobante,
                        principalTable: "Comprobantes",
                        principalColumn: "IdComprobante",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleComprobantes_Cuentas_IdCuenta",
                        column: x => x.IdCuenta,
                        principalTable: "Cuentas",
                        principalColumn: "IdCuenta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleComprobantes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmpresaMonedas",
                columns: table => new
                {
                    IdEmpresaMoneda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cambio = table.Column<float>(type: "real", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdMonedaPrincipal = table.Column<int>(type: "int", nullable: false),
                    IdMonedaAlternativa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaMonedas", x => x.IdEmpresaMoneda);
                    table.ForeignKey(
                        name: "FK_EmpresaMonedas_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpresaMonedas_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Monedas",
                columns: table => new
                {
                    IdMoneda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    EmpresaMonedaIdEmpresaMoneda = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monedas", x => x.IdMoneda);
                    table.ForeignKey(
                        name: "FK_Monedas_EmpresaMonedas_EmpresaMonedaIdEmpresaMoneda",
                        column: x => x.EmpresaMonedaIdEmpresaMoneda,
                        principalTable: "EmpresaMonedas",
                        principalColumn: "IdEmpresaMoneda");
                    table.ForeignKey(
                        name: "FK_Monedas_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_IdMoneda",
                table: "Comprobantes",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_IdUsuario",
                table: "Comprobantes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleComprobantes_IdComprobante",
                table: "DetalleComprobantes",
                column: "IdComprobante");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleComprobantes_IdCuenta",
                table: "DetalleComprobantes",
                column: "IdCuenta");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleComprobantes_IdUsuario",
                table: "DetalleComprobantes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaMonedas_IdEmpresa",
                table: "EmpresaMonedas",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaMonedas_IdMonedaAlternativa",
                table: "EmpresaMonedas",
                column: "IdMonedaAlternativa");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaMonedas_IdMonedaPrincipal",
                table: "EmpresaMonedas",
                column: "IdMonedaPrincipal");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaMonedas_IdUsuario",
                table: "EmpresaMonedas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Monedas_EmpresaMonedaIdEmpresaMoneda",
                table: "Monedas",
                column: "EmpresaMonedaIdEmpresaMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_Monedas_IdUsuario",
                table: "Monedas",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Comprobantes_Monedas_IdMoneda",
                table: "Comprobantes",
                column: "IdMoneda",
                principalTable: "Monedas",
                principalColumn: "IdMoneda",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmpresaMonedas_Monedas_IdMonedaAlternativa",
                table: "EmpresaMonedas",
                column: "IdMonedaAlternativa",
                principalTable: "Monedas",
                principalColumn: "IdMoneda",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmpresaMonedas_Monedas_IdMonedaPrincipal",
                table: "EmpresaMonedas",
                column: "IdMonedaPrincipal",
                principalTable: "Monedas",
                principalColumn: "IdMoneda",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmpresaMonedas_Monedas_IdMonedaAlternativa",
                table: "EmpresaMonedas");

            migrationBuilder.DropForeignKey(
                name: "FK_EmpresaMonedas_Monedas_IdMonedaPrincipal",
                table: "EmpresaMonedas");

            migrationBuilder.DropTable(
                name: "DetalleComprobantes");

            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropTable(
                name: "Monedas");

            migrationBuilder.DropTable(
                name: "EmpresaMonedas");
        }
    }
}
