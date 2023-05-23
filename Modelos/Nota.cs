using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models;

namespace Modelos;

public class Nota
{
    [Key]
    public int IdNota { get;           set; }
    public int      NroNota     { get; set; }
    public DateTime Fecha       { get; set; }
    public string   Descripcion { get; set; }
    public float    Total       { get; set; }
    public string   Tipo        { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get;    set; }
    public Usuario? Usuario { get; set; }


    [ForeignKey("Empresa")]
    public int IdEmpresa { get;    set; }

    [InverseProperty("Notas")]
    public Empresa? Empresa { get; set; }


    [ForeignKey("Comprobante")]
    public int IdComprobante { get;        set; }
    public Comprobante? Comprobante { get; set;}


}