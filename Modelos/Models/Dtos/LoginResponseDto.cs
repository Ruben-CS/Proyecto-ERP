namespace Modelos.Models.Dtos;

public class LoginResponseDto
{
    public UsuarioDto? Usuario { get; set; }
    public string?     Token   { get; set; }
}