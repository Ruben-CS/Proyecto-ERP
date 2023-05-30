using Modelos.Models.Dtos;

namespace ModuloContabilidadApi.Authentication.Interfaces;

public interface IAuthRepository
{
    bool IsUnique(string nombre);

    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    Task<UsuarioDto> Registrar(RegistroRequestDto requestDto);
}