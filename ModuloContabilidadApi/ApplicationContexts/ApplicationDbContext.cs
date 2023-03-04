using Microsoft.EntityFrameworkCore;
using ModuloContabilidadApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModuloContabilidadApi.ApplicationContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
                                    options) : base(options)
    {
    }

    public DbSet<Empresa> Empresas  { get; set; }
    public DbSet<Gestion> Gestiones { get; set; }
    public DbSet<Periodo> Periodos  { get; set; }
    public DbSet<Usuario> Usuarios  { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        #region Configuracion Empresa

        modelBuilder.Entity<Empresa>().HasIndex(i => i.Nit).IsUnique();
        modelBuilder.Entity<Empresa>().HasIndex(i => i.Sigla).IsUnique();
        modelBuilder.Entity<Empresa>().HasIndex(i => i.Nombre).IsUnique();

        #endregion


        #region Configruacion Gestion

        #endregion

        #region Configuracion Periodo

        #endregion

        #region Configuracion de Relaciones

        modelBuilder.Entity<Empresa>().HasOne(empresa => empresa.Usuario)
            .WithMany(
                usuario => usuario
                    .Empresas).HasForeignKey(empresa => empresa.UsuarioId);

        modelBuilder.Entity<Gestion>().HasOne(gestion => gestion.Usuario)
            .WithMany
                ().HasForeignKey(usuario => usuario.IdUsuario);

        modelBuilder.Entity<Gestion>().HasOne(gestion => gestion.Empresa)
            .WithMany
                ().HasForeignKey(empresa => empresa.IdEmpresa);

        modelBuilder.Entity<Periodo>().HasOne(periodo => periodo.Usuario)
            .WithMany().HasForeignKey(usuario => usuario.IdUsuario);

        modelBuilder.Entity<Periodo>().HasOne(periodo => periodo.Gestion)
            .WithMany().HasForeignKey(gestion => gestion.IdGestion);

        #endregion
    }
}