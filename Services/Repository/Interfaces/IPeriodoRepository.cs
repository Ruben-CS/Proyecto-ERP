using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IPeriodoRepository
{
    public Task<IEnumerable<PeriodoDto>> GetModelos(int gestionId);
    public Task<PeriodoDto> GetModelo(int modeloId);
    public Task<PeriodoDto> CreateUpdateModelDto(PeriodoDto periodoDto, int idGestion);
    public Task<bool> DeleteModel(int modeloId);

    public Task<PeriodoDto> UpdateModel(PeriodoDto periodoDto, int idGestion, int idPeriodo);
}