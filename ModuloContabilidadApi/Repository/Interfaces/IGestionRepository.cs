using ModuloContabilidadApi.Models.Dtos;

namespace ModuloContabilidadApi.Repository.Interfaces;

public interface IGestionRepository
{
    public Task<IEnumerable<GestionDto>> GetModelos();
    public Task<GestionDto> GetModelo(int modeloId);
    public Task<GestionDto> CreateUpdateModelDto(GestionDto gestionDto);
    public Task<bool> DeleteModel(int modeloId);
}