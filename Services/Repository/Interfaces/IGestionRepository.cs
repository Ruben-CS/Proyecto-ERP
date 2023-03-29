using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IGestionRepository
{
    public Task<IEnumerable<GestionDto>> GetModelos(int modeloId);
    public Task<GestionDto> CreateUpdateModelDto(GestionDto gestionDto, int idEmpresa);
    public Task<bool> DeleteModel(int modeloId);
}