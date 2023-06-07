using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;

namespace Modelos.Models.Dtos;

public sealed class EmpresaMonedaDto
{
    public EmpresaMonedaDto()
    {
        FechaRegistro = DateTime.Now;
    }
    public int IdEmpresaMoneda { get; set; }

    public decimal? Cambio { get; set; }

    public EstadoEmpresaMoneda Estado { get; set; } = EstadoEmpresaMoneda.Activo;

    public DateTime FechaRegistro { get; set; }

    public int IdEmpresa { get; set; }

    public int IdUsuario { get; set; } = 1;

    public Usuario? Usuario { get; set; }

    public Empresa? Empresa { get; set; }

    public int IdMonedaPrincipal { get; set; }

    public int? IdMonedaAlternativa { get; set; }

    public Moneda? MonedaPrincipal { get; set; }

    public Moneda? MonedaAlternativa { get; set; }

    public IEnumerable<Moneda>? Monedas { get; set; }
}