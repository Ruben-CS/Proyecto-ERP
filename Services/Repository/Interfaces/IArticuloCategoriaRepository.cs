using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IArticuloCategoriaRepository
{
    public Task<ArticuloCategoriaDto> CreateArticuloCategoria(ArticuloCategoriaDto dto);
}