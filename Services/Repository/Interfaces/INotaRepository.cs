using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface INotaRepository
{
    public Task<NotaDto>              CreateNota(NotaDto notaDto);
    public Task<bool>                 AnularNota(int     notaId);
    public Task<IEnumerable<NotaDto>> GetNotas(int       idEmpresa);
    public Task<NotaDto>              GetNota(int        notaId);
}