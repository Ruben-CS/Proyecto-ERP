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
        modelBuilder.Entity<Empresa>().Property(i => i.IdUsuario)
            .IsRequired(false);
        modelBuilder.Entity<Empresa>().HasIndex(i => i.IdUsuario);

        #endregion

        #region Configuracion de Usuario


        #endregion

        #region Configruacion Gestion

        #endregion

        #region Configuracion Periodo

        #endregion

        #region Configuracion de Relaciones

        modelBuilder.Entity<Empresa>().HasOne(empresa => empresa.Usuario)
            .WithMany(usuario => usuario.Empresas)
            .HasForeignKey(empresa => empresa.IdUsuario);



        modelBuilder.Entity<Gestion>().HasOne(gestion => gestion.Usuario)
            .WithMany(usuario => usuario.Gestiones)
            .HasForeignKey(gestion => gestion.IdUsuario)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Periodo>().HasOne(periodo => periodo.Usuario)
            .WithMany(usuario => usuario.Periodos)
            .HasForeignKey(periodo => periodo.IdGestion);

        modelBuilder.Entity<Periodo>().HasOne(periodo => periodo.Gestion)
            .WithMany(gestion => gestion.Periodos)
            .HasForeignKey(periodo => periodo.IdUsuario);

        #endregion
    }
}