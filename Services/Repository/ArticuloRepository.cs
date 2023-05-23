using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class ArticuloRepository : IArticuloRepository
{
    public Task<ArticuloDto>              CrearArticulo(ArticuloDto  dto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticuloDto>> ListarArticulo(int         idEmpresa)
    {
        throw new NotImplementedException();
    }

    public Task<ArticuloDto>              EditarArticulo(ArticuloDto dto, int idArticulo)
    {
        throw new NotImplementedException();
    }

    public Task<ArticuloDto>              GetSingleArticulo(int      idArticulo)
    {
        throw new NotImplementedException();
    }

    public Task<bool>                         BorrarArticulo(int         idArticulo)
    {
        throw new NotImplementedException();
    }
}