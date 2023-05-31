using System.Collections;
using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface ILoteRepository
{
    public Task<LoteDto>              CrearLote(LoteDto dto);
    public Task<IEnumerable<LoteDto>> GetLotes(int      articuloId);
    public Task<bool>                 EliminarLote(int  idLote);
}