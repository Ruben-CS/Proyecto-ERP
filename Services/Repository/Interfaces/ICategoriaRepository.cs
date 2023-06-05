using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface ICategoriaRepository
{
    public Task<CategoriaDto> CreateCategoria(CategoriaDto dto);

    public Task<CategoriaDto> EditarDto(CategoriaDto dto,int idCategoria);

    public Task<bool> EliminarDto(int idCategoria);

    public Task<IEnumerable<CategoriaDto>?> ListarCategoria(int idEmpresa);

    public Task<CategoriaDto> GetCategoriaById(int idCategoria);
}