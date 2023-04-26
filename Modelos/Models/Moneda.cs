using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class Moneda
{
    [Key]
    public int IdMoneda { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    [Required]
    public string Descripcion { get; set; } = null!;

    [Required]
    public string Abreviatura { get; set; } = null!;

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }


}