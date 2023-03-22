using Modelos.Models.Dtos;

namespace ModuloContabilidadApi.Models.Dtos;

public class LoginResponseDto
{
    public UsuarioDto Usuario { get; set; }
    public string     Token   { get; set; }
}