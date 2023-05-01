using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;

namespace Modelos.Models.Dtos;

public sealed class EmpresaMonedaDto
{
    public EmpresaMonedaDto()
    {
        FechaRegistro = DateTime.UtcNow;
    }
    public int IdEmpresaMoneda { get; set; }

    public float? Cambio { get; set; }

    public EstadoEmpresaMoneda Estado { get; set; } = EstadoEmpresaMoneda.Activo;

    public DateTime FechaRegistro { get; set; }

    public int IdEmpresa { get; set; }

    public Usuario? Usuario { get; set; }

    public Empresa? Empresa { get; set; }

    public int IdMonedaPrincipal { get; set; }

    public int? IdMonedaAlternativa { get; set; }

    public Moneda? MonedaPrincipal { get; set; }

    public Moneda? MonedaAlternativa { get; set; }

    public IEnumerable<Moneda>? Monedas { get; set; }
}