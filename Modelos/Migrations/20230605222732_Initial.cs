using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modelos.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sigla = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Niveles = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.IdEmpresa);
                    table.ForeignKey(
                        name: "FK_Empresas_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Articulo",
                columns: table => new
                {
                    IdArticulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
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
                name: "Cuentas",
                columns: table => new
                {
                    IdCuenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: true),
                    TipoCuenta = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdCuentaPadre = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.IdCuenta);
                    table.ForeignKey(
                        name: "FK_Cuentas_Cuentas_IdCuentaPadre",
                        column: x => x.IdCuentaPadre,
                        principalTable: "Cuentas",
                        principalColumn: "IdCuenta");
                    table.ForeignKey(
                        name: "FK_Cuentas_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gestiones",
                columns: table => new
                {
                    IdGestion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    EmpresaDtoIdEmpresa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestiones", x => x.IdGestion);
                    table.ForeignKey(
                        name: "FK_Gestiones_Empresas_EmpresaDtoIdEmpresa",
                        column: x => x.EmpresaDtoIdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa");
                    table.ForeignKey(
                        name: "FK_Gestiones_Usuarios_IdUsuario",
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

            migrationBuilder.CreateTable(
                name: "Periodos",
                columns: table => new
                {
                    IdPeriodo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdGestion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodos", x => x.IdPeriodo);
                    table.ForeignKey(
                        name: "FK_Periodos_Gestiones_IdGestion",
                        column: x => x.IdGestion,
                        principalTable: "Gestiones",
                        principalColumn: "IdGestion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Periodos_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comprobantes",
                columns: table => new
                {
                    IdComprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Serie = table.Column<int>(type: "int", nullable: true),
                    Glosa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tc = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    TipoComprobante = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdMoneda = table.Column<int>(type: "int", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobantes", x => x.IdComprobante);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa");
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
                    MontoDebe = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MontoHaber = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MontoDebeAlt = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MontoHaberAlt = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    NombreCuenta = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Nota",
                columns: table => new
                {
                    IdNota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroNota = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TipoNota = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    EstadoNota = table.Column<int>(type: "int", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdComprobante = table.Column<int>(type: "int", nullable: true)
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
                name: "Lotes",
                columns: table => new
                {
                    IdLote = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdArticulo = table.Column<int>(type: "int", nullable: false),
                    NroLote = table.Column<int>(type: "int", nullable: true),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    EstadoLote = table.Column<int>(type: "int", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    IdNota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotes", x => new { x.IdLote, x.IdArticulo });
                    table.ForeignKey(
                        name: "FK_Lotes_Articulo_IdArticulo",
                        column: x => x.IdArticulo,
                        principalTable: "Articulo",
                        principalColumn: "IdArticulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotes_Nota_IdNota",
                        column: x => x.IdNota,
                        principalTable: "Nota",
                        principalColumn: "IdNota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalle",
                columns: table => new
                {
                    IdArticulo = table.Column<int>(type: "int", nullable: false),
                    NroLote = table.Column<int>(type: "int", nullable: false),
                    IdNota = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalle", x => new { x.IdArticulo, x.NroLote, x.IdNota });
                    table.ForeignKey(
                        name: "FK_Detalle_Articulo_IdArticulo",
                        column: x => x.IdArticulo,
                        principalTable: "Articulo",
                        principalColumn: "IdArticulo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Detalle_Lotes_NroLote_IdArticulo",
                        columns: x => new { x.NroLote, x.IdArticulo },
                        principalTable: "Lotes",
                        principalColumns: new[] { "IdLote", "IdArticulo" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Detalle_Nota_IdNota",
                        column: x => x.IdNota,
                        principalTable: "Nota",
                        principalColumn: "IdNota",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmpresaMonedas",
                columns: table => new
                {
                    IdEmpresaMoneda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cambio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
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
                name: "IX_Comprobantes_IdEmpresa",
                table: "Comprobantes",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_IdMoneda",
                table: "Comprobantes",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_IdUsuario",
                table: "Comprobantes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdCuentaPadre",
                table: "Cuentas",
                column: "IdCuentaPadre");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdEmpresa",
                table: "Cuentas",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_IdNota",
                table: "Detalle",
                column: "IdNota");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_NroLote_IdArticulo",
                table: "Detalle",
                columns: new[] { "NroLote", "IdArticulo" });

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
                name: "IX_Empresas_IdUsuario",
                table: "Empresas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Nombre",
                table: "Empresas",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Sigla",
                table: "Empresas",
                column: "Sigla");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_EmpresaDtoIdEmpresa",
                table: "Gestiones",
                column: "EmpresaDtoIdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Gestiones_IdUsuario",
                table: "Gestiones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_IdArticulo",
                table: "Lotes",
                column: "IdArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_IdNota",
                table: "Lotes",
                column: "IdNota");

            migrationBuilder.CreateIndex(
                name: "IX_Monedas_EmpresaMonedaIdEmpresaMoneda",
                table: "Monedas",
                column: "EmpresaMonedaIdEmpresaMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_Monedas_IdUsuario",
                table: "Monedas",
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

            migrationBuilder.CreateIndex(
                name: "IX_Periodos_IdGestion",
                table: "Periodos",
                column: "IdGestion");

            migrationBuilder.CreateIndex(
                name: "IX_Periodos_IdUsuario",
                table: "Periodos",
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
                name: "FK_EmpresaMonedas_Empresas_IdEmpresa",
                table: "EmpresaMonedas");

            migrationBuilder.DropForeignKey(
                name: "FK_EmpresaMonedas_Usuarios_IdUsuario",
                table: "EmpresaMonedas");

            migrationBuilder.DropForeignKey(
                name: "FK_Monedas_Usuarios_IdUsuario",
                table: "Monedas");

            migrationBuilder.DropForeignKey(
                name: "FK_EmpresaMonedas_Monedas_IdMonedaAlternativa",
                table: "EmpresaMonedas");

            migrationBuilder.DropForeignKey(
                name: "FK_EmpresaMonedas_Monedas_IdMonedaPrincipal",
                table: "EmpresaMonedas");

            migrationBuilder.DropTable(
                name: "ArticuloCategoria");

            migrationBuilder.DropTable(
                name: "Detalle");

            migrationBuilder.DropTable(
                name: "DetalleComprobantes");

            migrationBuilder.DropTable(
                name: "Periodos");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Lotes");

            migrationBuilder.DropTable(
                name: "Cuentas");

            migrationBuilder.DropTable(
                name: "Gestiones");

            migrationBuilder.DropTable(
                name: "Articulo");

            migrationBuilder.DropTable(
                name: "Nota");

            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Monedas");

            migrationBuilder.DropTable(
                name: "EmpresaMonedas");
        }
    }
}
