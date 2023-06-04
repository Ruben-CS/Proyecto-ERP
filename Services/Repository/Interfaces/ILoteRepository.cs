using System.Collections;
using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface ILoteRepository
{
    public Task<LoteDto>              CrearLote(LoteDto dto, int idArticulo);
    public Task<IEnumerable<LoteDto>> GetLotes(int      idArticulo);
    public Task<bool>                 EliminarLote(int  idLote, int idArticulo);
}