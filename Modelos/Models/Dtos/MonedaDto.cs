namespace Modelos.Models.Dtos;

public class MonedaDto
{
    public int IdMoneda { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Abreviatura { get; set; } = null!;

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }
}