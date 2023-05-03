using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;

namespace Modelos.Models;

public sealed class EmpresaMoneda
{
    public EmpresaMoneda()
    {
        FechaRegistro = DateTime.UtcNow;
    }

    [Key]
    public int IdEmpresaMoneda { get; set; }

    public float? Cambio { get; set; }

    public EstadoEmpresaMoneda Estado { get; set; } = EstadoEmpresaMoneda.Activo;

    public DateTime FechaRegistro { get; set; }

    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; } = 1;


    [InverseProperty("EmpresaMonedas")]
    public Usuario? Usuario { get; set; }

    [InverseProperty("EmpresaMonedas")]
    public Empresa? Empresa { get; set; }

    [ForeignKey("MonedaPrincipal")]
    public int IdMonedaPrincipal { get; set; }

    [ForeignKey("MonedaAlternativa")]
    public int? IdMonedaAlternativa { get; set; }

    public Moneda? MonedaPrincipal { get; set; }

    public Moneda? MonedaAlternativa { get; set; }

    public IEnumerable<Moneda>? Monedas { get; set; }
}