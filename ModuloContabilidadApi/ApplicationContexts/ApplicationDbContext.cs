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
            .WithMany(usuario => usuario.Empresas).HasForeignKey(empresa => empresa.UsuarioId);


        modelBuilder.Entity<Gestion>().HasOne(gestion => gestion.Empresa)
            .WithMany(empresa => empresa.Gestiones).HasForeignKey(gestion => gestion
            .IdEmpresa)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Gestion>().HasOne(gestion => gestion.Usuario)
            .WithMany(usuario => usuario.Gestiones)
            .HasForeignKey(gestion => gestion.IdUsuario)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Periodo>().HasOne(periodo => periodo.Usuario)
            .WithMany(usuario => usuario.Periodos).HasForeignKey(periodo => periodo
            .IdGestion);

        modelBuilder.Entity<Periodo>().HasOne(periodo => periodo.Gestion)
            .WithMany(gestion => gestion.Periodos).HasForeignKey(periodo => periodo
            .IdUsuario);

        #endregion
    }
}