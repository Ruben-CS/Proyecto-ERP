using Modelos.Models.Enums;

namespace Modelos.Models.Dtos;

public class CuentaDto
{
    public int IdCuenta { get; set; }

    public string? Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public int? Nivel { get; set; }

    public string? TipoCuenta { get; set; }

    public int IdUsuario { get; set; } = 1;

    public int IdEmpresa { get; set; }

    public Empresa? Empresa { get; set; }

    public int? IdCuentaPadre { get; set; }

    public virtual Cuenta? IdCuentaPadreNavigation { get; set; }

    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;

    public ICollection<Cuenta>? CuentasHijas { get; set; }
}