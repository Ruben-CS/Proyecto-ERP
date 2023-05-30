using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IArticuloRepository
{
    public Task<ArticuloDto>              CrearArticulo(ArticuloDto  dto);
    public Task<IEnumerable<ArticuloDto>> ListarArticulo(int         idEmpresa);
    public Task<ArticuloDto>              EditarArticulo(ArticuloDto dto, int idArticulo);
    public Task<ArticuloDto>              GetSingleArticulo(int      idArticulo);
    public Task<bool>                     BorrarArticulo(int         idArticulo);
}