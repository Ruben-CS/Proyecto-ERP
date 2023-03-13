using ModuloContabilidadApi.Models.Dtos;

namespace ModuloContabilidadApi.Repository.Interfaces;

public interface IAuthRepository
{
    bool IsUnique(string nombre);

    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    Task<UsuarioDto> Registrar(RegistroRequestDto requestDto);
}