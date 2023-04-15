﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Modelos.ApplicationContexts;

#nullable disable

namespace Modelos.Migrations
{
    [DbContext(typeof(_applicationDbContext))]
    [Migration("20230403005349_tinydetails")]
    partial class tinydetails
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

            modelBuilder.Entity("Modelos.Models.Empresa", b =>
                {
                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("Empresas")
                        .HasForeignKey("IdUsuario");

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

            modelBuilder.Entity("Modelos.Models.Periodo", b =>
                {
                    b.HasOne("Modelos.Models.Usuario", "Usuario")
                        .WithMany("Periodos")
                        .HasForeignKey("IdGestion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Modelos.Models.Gestion", "Gestion")
                        .WithMany("Periodos")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gestion");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Modelos.Models.Empresa", b =>
                {
                    b.Navigation("Gestiones");
                });

            modelBuilder.Entity("Modelos.Models.Gestion", b =>
                {
                    b.Navigation("Periodos");
                });

            modelBuilder.Entity("Modelos.Models.Usuario", b =>
                {
                    b.Navigation("Empresas");

                    b.Navigation("Gestiones");

                    b.Navigation("Periodos");
                });
#pragma warning restore 612, 618
        }
    }
}
