using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class FinalProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articulo",
                columns: table => new
                {
                    IdArticulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioVenta = table.Column<float>(type: "real", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articulo", x => x.IdArticulo);
                    table.ForeignKey(
                        name: "FK_Articulo_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa");
                    table.ForeignKey(
                        name: "FK_Articulo_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdCategoriaPadre = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.IdCategoria);
                    table.ForeignKey(
                        name: "FK_Categoria_Categoria_IdCategoriaPadre",
                        column: x => x.IdCategoriaPadre,
                        principalTable: "Categoria",
                        principalColumn: "IdCategoria");
                    table.ForeignKey(
                        name: "FK_Categoria_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa");
                    table.ForeignKey(
                        name: "FK_Categoria_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Nota",
                columns: table => new
                {
                    IdNota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroNota = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdComprobante = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nota", x => x.IdNota);
                    table.ForeignKey(
                        name: "FK_Nota_Comprobantes_IdComprobante",
                        column: x => x.IdComprobante,
                        principalTable: "Comprobantes",
                        principalColumn: "IdComprobante");
                    table.ForeignKey(
                        name: "FK_Nota_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa");
                    table.ForeignKey(
                        name: "FK_Nota_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "ArticuloCategoria",
                columns: table => new
                {
                    IdArticulo = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    NombreCategoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticuloCategoria", x => new { x.IdArticulo, x.IdCategoria });
                    table.ForeignKey(
                        name: "FK_ArticuloCategoria_Articulo_IdArticulo",
                        column: x => x.IdArticulo,
                        principalTable: "Articulo",
                        principalColumn: "IdArticulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticuloCategoria_Categoria_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_IdEmpresa",
                table: "Articulo",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_IdUsuario",
                table: "Articulo",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ArticuloCategoria_IdCategoria",
                table: "ArticuloCategoria",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_IdCategoriaPadre",
                table: "Categoria",
                column: "IdCategoriaPadre");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_IdEmpresa",
                table: "Categoria",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_IdUsuario",
                table: "Categoria",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdComprobante",
                table: "Nota",
                column: "IdComprobante");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdEmpresa",
                table: "Nota",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdUsuario",
                table: "Nota",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticuloCategoria");

            migrationBuilder.DropTable(
                name: "Nota");

            migrationBuilder.DropTable(
                name: "Articulo");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
