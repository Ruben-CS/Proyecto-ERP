using System.Collections;
using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface ILoteRepository
{
    public Task<LoteDto>              CrearLote(LoteDto dto, int idNota);
    public Task<IEnumerable<LoteDto>> GetLotes(int      idNota);
    public Task<IEnumerable<LoteDto>> GetLotesPorArticulo(int idArticulo);
}