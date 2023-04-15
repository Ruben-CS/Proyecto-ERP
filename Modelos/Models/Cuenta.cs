using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;

namespace Modelos.Models;

public class Cuenta
{
    [Key]
    public int IdCuenta { get; set; }

    public string? Codigo { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    public int? Nivel { get; set; }

    public string? TipoCuenta { get; set; }

    [ForeignKey("Empresa")]
    public int IdUsuario { get; set; } = 1;

    public int IdEmpresa { get; set; }

    public Empresa? Empresa { get; set; }

    public int? IdCuentaPadre { get; set; }

    public virtual Cuenta? IdCuentaPadreNavigation { get; set; }

    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;

    public ICollection<Cuenta>? CuentasHijas { get; set; }
}