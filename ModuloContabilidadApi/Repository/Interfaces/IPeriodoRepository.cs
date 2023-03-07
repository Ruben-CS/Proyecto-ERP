using ModuloContabilidadApi.Models.Dtos;

namespace ModuloContabilidadApi.Repository.Interfaces;

public interface IPeriodoRepository
{
    public Task<IEnumerable<PeriodoDto>> GetModelos();
    public Task<PeriodoDto> GetModelo(Guid modeloId);
    public Task<PeriodoDto> CreateUpdateModelDto(PeriodoDto periodoDto);
    public Task<bool> DeleteModel(Guid modeloId);
}