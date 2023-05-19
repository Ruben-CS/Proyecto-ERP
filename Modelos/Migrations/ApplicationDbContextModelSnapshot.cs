﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Modelos.ApplicationContexts;

#nullable disable

namespace ModuloContabilidadApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Modelos.Models.Comprobante", b =>
                {
                    b.Property<int>("IdComprobante")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdComprobante"));

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("Glosa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int>("IdMoneda")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<int?>("Serie")
                        .HasColumnType("int");

                    b.Property<decimal?>("Tc")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("TipoComprobante")
                        .HasColumnType("int");

                    b.HasKey("IdComprobante");

                    b.HasIndex("IdEmpresa");

                    b.HasIndex("IdMoneda");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Comprobantes");
                });

            modelBuilder.Entity("Modelos.Models.Cuenta", b =>
                {
                    b.Property<int>("IdCuenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCuenta"));

                    b.Property<string>("Codigo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<int?>("IdCuentaPadre")
                        .HasColumnType("int");

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<int?>("Nivel")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoCuenta")
                        .HasColumnType("int");

                    b.HasKey("IdCuenta");

                    b.HasIndex("IdCuentaPadre");

                    b.HasIndex("IdEmpresa");

                    b.ToTable("Cuentas");
                });

            modelBuilder.Entity("Modelos.Models.DetalleComprobante", b =>
                {
                    b.Property<int>("IdDetalleComprobante")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetalleComprobante"));

                    b.Property<string>("Glosa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdComprobante")
                        .HasColumnType("int");

                    b.Property<int>("IdCuenta")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<decimal>("MontoDebe")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MontoDebeAlt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MontoHaber")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MontoHaberAlt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NombreCuenta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.HasKey("IdDetalleComprobante");

                    b.HasIndex("IdComprobante");

                    b.HasIndex("IdCuenta");

                    b.HasIndex("IdUsuario");

                    b.ToTable("DetalleComprobantes");
                });

            modelBuilder.Entity("Modelos.Models.Empresa", b =>
                {
                    b.Property<int>("IdEmpresa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmpresa"));

                    b.Property<string>("Correo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nit")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Niveles")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Sigla")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEmpresa");

                    b.HasIndex("IdUsuario");

                    b.HasIndex("Nit")
                        .IsUnique();

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.HasIndex("Sigla")
                        .IsUnique();

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("Modelos.Models.EmpresaMoneda", b =>
                {
                    b.Property<int>("IdEmpresaMoneda")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmpresaMoneda"));

                    b.Property<decimal?>("Cambio")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int?>("IdMonedaAlternativa")
                        .HasColumnType("int");

                    b.Property<int>("IdMonedaPrincipal")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("IdEmpresaMoneda");

                    b.HasIndex("IdEmpresa");

                    b.HasIndex("IdMonedaAlternativa");

                    b.HasIndex("IdMonedaPrincipal");

                    b.HasIndex("IdUsuario");

                    b.ToTable("EmpresaMonedas");
                });

            modelBuilder.Entity("Modelos.Models.Gestion", b =>
                {
                    b.Property<int>("IdGestion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdGestion"));

                    b.Property<int?>("EmpresaDtoIdEmpresa")
                        .HasColumnType("int");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdGestion");

                    b.HasIndex("EmpresaDtoIdEmpresa");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Gestiones");
                });

            modelBuilder.Entity("Modelos.Models.Moneda", b =>
                {
                    b.Property<int>("IdMoneda")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMoneda"));

                    b.Property<string>("Abreviatura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmpresaMonedaIdEmpresaMoneda")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdMoneda");

                    b.HasIndex("EmpresaMonedaIdEmpresaMoneda");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Monedas");
                });

            modelBuilder.Entity("Modelos.Models.Periodo", b =>
                {
                    b.Property<int>("IdPeriodo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPeriodo"));

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdGestion")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPeriodo");

                    b.HasIndex("IdGestion");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Periodos");
                });

            modelBuilder.Entity("Modelos.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUsuario");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Modelos.Models.Comprobante", b =>
                {
                    b.HasOne("Modelos.Models.Empresa", "Empresa")
                        .WithMany("Comprobantes")
                        .HasForeignKey("IdEmpresa")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Moneda", "Moneda")
                        .WithMany()
                        .HasForeignKey("IdMoneda")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("Comprobantes")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Empresa");

                    b.Navigation("Moneda");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.Cuenta", b =>
                {
                    b.HasOne("Modelos.Models.Cuenta", "IdCuentaPadreNavigation")
                        .WithMany("CuentasHijas")
                        .HasForeignKey("IdCuentaPadre")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Modelos.Models.Empresa", "Empresa")
                        .WithMany("Cuentas")
                        .HasForeignKey("IdEmpresa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Empresa");

                    b.Navigation("IdCuentaPadreNavigation");
                });

            modelBuilder.Entity("Modelos.Models.DetalleComprobante", b =>
                {
                    b.HasOne("Modelos.Models.Comprobante", "Comprobante")
                        .WithMany("DetalleComprobantes")
                        .HasForeignKey("IdComprobante")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Cuenta", "Cuenta")
                        .WithMany("DetalleComprobantes")
                        .HasForeignKey("IdCuenta")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("DetalleComprobantes")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Comprobante");

                    b.Navigation("Cuenta");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.Empresa", b =>
                {
                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("Empresas")
                        .HasForeignKey("IdUsuario");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.EmpresaMoneda", b =>
                {
                    b.HasOne("Modelos.Models.Empresa", "Empresa")
                        .WithMany("EmpresaMonedas")
                        .HasForeignKey("IdEmpresa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Moneda", "MonedaAlternativa")
                        .WithMany()
                        .HasForeignKey("IdMonedaAlternativa")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Modelos.Models.Moneda", "MonedaPrincipal")
                        .WithMany()
                        .HasForeignKey("IdMonedaPrincipal")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("EmpresaMonedas")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Empresa");

                    b.Navigation("MonedaAlternativa");

                    b.Navigation("MonedaPrincipal");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.Gestion", b =>
                {
                    b.HasOne("Modelos.Models.Empresa", "EmpresaDto")
                        .WithMany("Gestiones")
                        .HasForeignKey("EmpresaDtoIdEmpresa");

                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("Gestiones")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EmpresaDto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.Moneda", b =>
                {
                    b.HasOne("Modelos.Models.EmpresaMoneda", null)
                        .WithMany("Monedas")
                        .HasForeignKey("EmpresaMonedaIdEmpresaMoneda");

                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("Monedas")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.Periodo", b =>
                {
                    b.HasOne("Modelos.Models.Gestion", "Gestion")
                        .WithMany("Periodos")
                        .HasForeignKey("IdGestion")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("Periodos")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Gestion");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.Comprobante", b =>
                {
                    b.Navigation("DetalleComprobantes");
                });

            modelBuilder.Entity("Modelos.Models.Cuenta", b =>
                {
                    b.Navigation("CuentasHijas");

                    b.Navigation("DetalleComprobantes");
                });

            modelBuilder.Entity("Modelos.Models.Empresa", b =>
                {
                    b.Navigation("Comprobantes");

                    b.Navigation("Cuentas");

                    b.Navigation("EmpresaMonedas");

                    b.Navigation("Gestiones");
                });

            modelBuilder.Entity("Modelos.Models.EmpresaMoneda", b =>
                {
                    b.Navigation("Monedas");
                });

            modelBuilder.Entity("Modelos.Models.Gestion", b =>
                {
                    b.Navigation("Periodos");
                });

            modelBuilder.Entity("Modelos.Models.Usuario", b =>
                {
                    b.Navigation("Comprobantes");

                    b.Navigation("DetalleComprobantes");

                    b.Navigation("EmpresaMonedas");

                    b.Navigation("Empresas");

                    b.Navigation("Gestiones");

                    b.Navigation("Monedas");

                    b.Navigation("Periodos");
                });
#pragma warning restore 612, 618
        }
    }
}
