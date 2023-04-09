using Microsoft.EntityFrameworkCore;
using Modelos.Models;
using ModuloContabilidadApi.Models;

namespace Modelos.ApplicationContexts;

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

        modelBuilder.Entity<Empresa>()
                    .HasOne(empresa => empresa.Usuario)
                    .WithMany(usuario => usuario.Empresas)
                    .HasForeignKey(empresa => empresa.IdUsuario);


        modelBuilder.Entity<Gestion>()
                    .HasOne(gestion => gestion.Usuario)
                    .WithMany(usuario => usuario.Gestiones)
                    .HasForeignKey(gestion => gestion.IdUsuario)
                    .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Periodo>()
                    .HasOne(p => p.Usuario)
                    .WithMany(u => u.Periodos)
                    .HasForeignKey(p => p.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Periodo>()
                    .HasOne(p => p.Gestion)
                    .WithMany(g => g.Periodos)
                    .HasForeignKey(p => p.IdGestion)
                    .OnDelete(DeleteBehavior.Restrict);

        #endregion
    }
}