using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IGestionRepository
{
    public Task<IEnumerable<GestionDto>> GetModelos(int modeloId);
    public Task<GestionDto> CreateUpdateModelDto(GestionDto gestionDto, int idEmpresa);
    public Task<bool> DeleteModel(int modeloId);

    public Task<GestionDto> UpdateModel(GestionDto gestionDto, int idGestion);

    public Task<GestionDto> GetModelo(int modeloId);
}