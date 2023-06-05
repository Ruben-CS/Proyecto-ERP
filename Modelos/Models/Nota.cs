using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;

namespace Modelos.Models;

public class Nota
{
    [Key]
    public int IdNota { get; set; }

    public int?      NroNota     { get; set; }
    public DateTime Fecha       { get; set; }
    public string   Descripcion { get; set; }
    public decimal    Total       { get; set; }
    public TipoNota TipoNota    { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    public EstadoNota EstadoNota { get; set; }

    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    [InverseProperty("Notas")]
    public Empresa? Empresa { get; set; }


    [ForeignKey("Comprobante")]
    public int? IdComprobante { get; set; }

    public Comprobante? Comprobante { get; set; }

    public ICollection<Lote> Lotes { get; set; }
}