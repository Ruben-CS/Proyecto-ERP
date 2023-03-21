using System.Text.Json.Serialization;

namespace Modelos.Models.Dtos;

public class UsuarioDto
{
    public int IdUsuario { get; set; }

    [JsonPropertyName("nombre")]

    public string Nombre { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}