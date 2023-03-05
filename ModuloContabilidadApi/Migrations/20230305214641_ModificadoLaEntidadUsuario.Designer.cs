﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModuloContabilidadApi.ApplicationContexts;

#nullable disable

namespace ModuloContabilidadApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230305214641_ModificadoLaEntidadUsuario")]
    partial class ModificadoLaEntidadUsuario
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ModuloContabilidadApi.Models.Empresa", b =>
                {
                    b.Property<Guid>("IdEmpresa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdEmpresa");

                    b.HasIndex("Nit")
                        .IsUnique();

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.HasIndex("Sigla")
                        .IsUnique();

                    b.HasIndex("UsuarioId");

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Gestion", b =>
                {
                    b.Property<Guid>("IdGestion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("IdEmpresa")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdUsuario")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdGestion");

                    b.HasIndex("IdEmpresa");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Gestiones");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Periodo", b =>
                {
                    b.Property<Guid>("IdPeriodo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("IdGestion")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdUsuario")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPeriodo");

                    b.HasIndex("IdGestion");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Periodos");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Usuario", b =>
                {
                    b.Property<Guid>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUsuario");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Empresa", b =>
                {
                    b.HasOne("ModuloContabilidadApi.Models.Usuario", "Usuario")
                        .WithMany("Empresas")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Gestion", b =>
                {
                    b.HasOne("ModuloContabilidadApi.Models.Empresa", "Empresa")
                        .WithMany("Gestiones")
                        .HasForeignKey("IdEmpresa")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ModuloContabilidadApi.Models.Usuario", "Usuario")
                        .WithMany("Gestiones")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Empresa");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Periodo", b =>
                {
                    b.HasOne("ModuloContabilidadApi.Models.Usuario", "Usuario")
                        .WithMany("Periodos")
                        .HasForeignKey("IdGestion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModuloContabilidadApi.Models.Gestion", "Gestion")
                        .WithMany("Periodos")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gestion");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Empresa", b =>
                {
                    b.Navigation("Gestiones");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Gestion", b =>
                {
                    b.Navigation("Periodos");
                });

            modelBuilder.Entity("ModuloContabilidadApi.Models.Usuario", b =>
                {
                    b.Navigation("Empresas");

                    b.Navigation("Gestiones");

                    b.Navigation("Periodos");
                });
#pragma warning restore 612, 618
        }
    }
}
