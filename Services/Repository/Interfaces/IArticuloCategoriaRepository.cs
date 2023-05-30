using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IArticuloCategoriaRepository
{
    public Task<ArticuloCategoriaDto> CreateArticuloCategoria(ArticuloCategoriaDto dto,
    int idArticulo, int idCategoria);

    public Task<IEnumerable<ArticuloCategoriaDto>> GetArticuloDetalles(
        int idArticulo);

    public Task<ArticuloCategoriaDto> EditArticuloCategoria(
        ArticuloCategoriaDto dto, int idArticulo, int idCategoria);
}