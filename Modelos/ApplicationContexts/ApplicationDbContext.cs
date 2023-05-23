using Microsoft.EntityFrameworkCore;
using Modelos.Models;

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

    public DbSet<Moneda> Monedas { get; set; }

    public DbSet<EmpresaMoneda> EmpresaMonedas { get; set; }

    public DbSet<DetalleComprobante> DetalleComprobantes { get; set; }

    public DbSet<Comprobante> Comprobantes { get; set; }

    public DbSet<Cuenta> Cuentas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Comprobante>(entity =>
        {
            entity.Property(e => e.Tc)
                  .HasColumnType("decimal(18,4)"); // or whatever precision and scale you need
        });

        modelBuilder.Entity<DetalleComprobante>(entity =>
        {
            entity.Property(e => e.NombreCuenta)
                  .HasColumnType("nvarchar(max)");
        });

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

        #region Configuracion EmpresaMoneda
        modelBuilder.Entity<EmpresaMoneda>()
                    .Property(em => em.Cambio)
                    .HasPrecision(18, 2);


        #endregion

        #region Configuracion de Relaciones

        modelBuilder.Entity<Empresa>()
                    .HasOne(empresa => empresa.Usuario)
                    .WithMany(usuario => usuario.Empresas)
                    .HasForeignKey(empresa => empresa.IdUsuario);

        modelBuilder.Entity<Empresa>().HasMany(e => e.Cuentas)
                    .WithOne(c => c.Empresa)
                    .HasForeignKey(c => c.IdEmpresa)
                    .OnDelete(DeleteBehavior.Cascade);


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

        modelBuilder.Entity<Cuenta>().HasOne(cuenta => cuenta.Empresa)
                    .WithMany(empresa => empresa.Cuentas)
                    .HasForeignKey(cuenta => cuenta.IdEmpresa);

        modelBuilder.Entity<Cuenta>().HasOne(cuenta => cuenta.IdCuentaPadreNavigation)
                    .WithMany(cuenta => cuenta.CuentasHijas)
                    .HasForeignKey(cuenta => cuenta.IdCuentaPadre)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Moneda>().HasOne(m => m.Usuario)
                    .WithMany(u => u.Monedas)
                    .HasForeignKey(m => m.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Comprobante>().HasOne(c => c.Usuario)
                    .WithMany(u => u.Comprobantes)
                    .HasForeignKey(c => c.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comprobante>().HasOne(c => c.Moneda)
                    .WithMany()
                    .HasForeignKey(c => c.IdMoneda)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comprobante>().HasMany(c => c.DetalleComprobantes)
                    .WithOne(d => d.Comprobante)
                    .HasForeignKey(d => d.IdComprobante)
                    .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<DetalleComprobante>().HasOne(d => d.Usuario)
                    .WithMany(u => u.DetalleComprobantes)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleComprobante>().HasOne(d => d.Comprobante)
                    .WithMany(c => c.DetalleComprobantes)
                    .HasForeignKey(d => d.IdComprobante)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleComprobante>().HasOne(d => d.Cuenta)
                    .WithMany(c => c.DetalleComprobantes)
                    .HasForeignKey(d => d.IdCuenta)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>().HasMany(u => u.EmpresaMonedas)
                    .WithOne(em => em.Usuario)
                    .HasForeignKey(em => em.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>().HasMany(u => u.Comprobantes)
                    .WithOne(c => c.Usuario)
                    .HasForeignKey(c => c.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>().HasMany(u => u.DetalleComprobantes)
                    .WithOne(d => d.Usuario)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>().HasMany(u => u.Monedas)
                    .WithOne(m => m.Usuario)
                    .HasForeignKey(m => m.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmpresaMoneda>().HasOne(em => em.Usuario)
                    .WithMany(u => u.EmpresaMonedas)
                    .HasForeignKey(em => em.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmpresaMoneda>().HasOne(em => em.Empresa)
                    .WithMany(e => e.EmpresaMonedas)
                    .HasForeignKey(em => em.IdEmpresa)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmpresaMoneda>().HasOne(em => em.MonedaPrincipal)
                    .WithMany()
                    .HasForeignKey(em => em.IdMonedaPrincipal)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmpresaMoneda>().HasOne(em => em.MonedaAlternativa)
                    .WithMany()
                    .HasForeignKey(em => em.IdMonedaAlternativa)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comprobante>().HasOne(comprobante => comprobante.Empresa)
                    .WithMany(empresa => empresa.Comprobantes)
                    .HasForeignKey(comprobante => comprobante.IdEmpresa)
                    .OnDelete(DeleteBehavior.NoAction);

        #endregion


        // Relación de uno a muchos entre Empresa y Categoria
            modelBuilder.Entity<Categoria>()
                .HasOne(categoria => categoria.Empresa)
                .WithMany(empresa => empresa.HijosCategorias)
                .HasForeignKey(categoria => categoria.IdEmpresa)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación recursiva de uno a muchos entre Categoria y Categoria
            modelBuilder.Entity<Categoria>()
                .HasOne(categoria => categoria.IdCategoriaPadreNavigation)
                .WithMany(categoria => categoria.HijosCategoria)
                .HasForeignKey(categoria => categoria.IdCategoriaPadre)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación entre usuario y categoria
            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.HijosCategorias)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación entre Empresa y Usuario
            modelBuilder.Entity<Empresa>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Empresas) // Asegúrate de que "Empresas" es la colección correcta en la entidad Usuario
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction);
            // Relación entre usuario y categoria
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.HijosCategorias)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction); // añade esta línea para evitar las eliminaciones en cascada


               //configurando articulo
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Articulos)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction); // añade esta línea para evitar las eliminaciones en cascada
            modelBuilder.Entity<Articulo>()
               .HasOne(categoria => categoria.Empresa)
               .WithMany(empresa => empresa.Articulos)
               .HasForeignKey(categoria => categoria.IdEmpresa)
               .OnDelete(DeleteBehavior.NoAction);


            //configurando articulocategoria
            modelBuilder.Entity<ArticuloCategoria>()
        .HasKey(ac => new { ac.IdArticulo, ac.IdCategoria });

            modelBuilder.Entity<ArticuloCategoria>()
                .HasOne(ac => ac.Articulo)
                .WithMany(a => a.ArticuloCategorias)
                .HasForeignKey(ac => ac.IdArticulo);

            modelBuilder.Entity<ArticuloCategoria>()
                .HasOne(ac => ac.Categoria)
                .WithMany(c => c.ArticuloCategorias)
                .HasForeignKey(ac => ac.IdCategoria);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Notas)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Nota>()
               .HasOne(categoria => categoria.Empresa)
               .WithMany(empresa => empresa.Notas)
               .HasForeignKey(categoria => categoria.IdEmpresa)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Nota>()
               .HasOne(categoria => categoria.Comprobante)
               .WithMany(empresa => empresa.Notas)
               .HasForeignKey(categoria => categoria.IdComprobante)
               .OnDelete(DeleteBehavior.NoAction);
    }
}